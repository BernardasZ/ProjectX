using Api.Helpers;
using Api.Models.Login;
using Api.Models.User;
using Api.Options;
using AutoMapper;
using Microsoft.Extensions.Options;
using Persistence.Entities.ProjectX;
using Persistence.Filters;
using Persistence.Repositories;
using System;
using System.Linq;
using System.Net.Mail;

namespace Api.Services.Concrete;

public class UserRecoverService : IUserRecoverService
{
	private readonly IRepository<User> _userRepository;
	private readonly IAesCryptoHelper _aesCryptoHelper;
	private readonly IClientContextScraper _httpContextScraper;
	private readonly IHashCryptoHelper _cryptoHelper;
	private readonly IOptionsMonitor<OptionManager> _optionManager;
	private readonly IMessageService _messageService;
	private readonly IUserServiceValidationHelper _validationHelper;
	private readonly IMapper _mapper;

	public UserRecoverService(
		IRepository<User> userDataRepository,
		IAesCryptoHelper aesCryptoHelper,
		IClientContextScraper clientContextScraper,
		IHashCryptoHelper hashCryptoHelper,
		IOptionsMonitor<OptionManager> optionManager,
		IMessageService messageService,
		IUserServiceValidationHelper userServiceValidationHelper,
		IMapper mapper)
	{
		_userRepository = userDataRepository;
		_aesCryptoHelper = aesCryptoHelper;
		_httpContextScraper = clientContextScraper;
		_cryptoHelper = hashCryptoHelper;
		_optionManager = optionManager;
		_messageService = messageService;
		_validationHelper = userServiceValidationHelper;
		_mapper = mapper;
	}

	public UserModel ChangePassword(UserChangePasswordModel model)
	{
		var filter = new UserEntityFilter
		{
			Id = int.Parse(_aesCryptoHelper.DecryptString(_httpContextScraper.GetClientClaimsIdentityName())),
			Email = model.Email,
			PassHash = _cryptoHelper.HashString(model.OldPassword)
		};

		var user = GetUserByFilter(filter);

		user.PassHash = _cryptoHelper.HashString(model.NewPassword);
		user.FailedLoginCount = 0;

		return _mapper.Map<UserModel>(_userRepository.Update(user));
	}

	public UserModel ResetPassword(UserResetPasswordModel model)
	{
		var user = GetUserByFilter(new UserEntityFilter { TokenHash = model.Token });

		_validationHelper.CheckIfPasswordResetTokenValid(user);

		user.PassHash = _cryptoHelper.HashString(model.NewPassword);
		user.IsTokenUsed = true;
		user.FailedLoginCount = 0;

		return _mapper.Map<UserModel>(_userRepository.Update(user));
	}

	public bool InitUserPasswordReset(InitPasswordResetModel model)
	{
		var user = GetUserByFilter(new UserEntityFilter { Email = model.Email });

		user.IsTokenUsed = false;
		user.TokenExpirationTime = DateTime.Now.AddMinutes(
			_optionManager.CurrentValue.AppSettings.PasswordResetExpirationInMin);
		user.TokenHash = _cryptoHelper.HashString(model.Email);

		_userRepository.Update(user);

		SendEmail(model, user.TokenHash);

		return true;
	}

	private User GetUserByFilter(IEntityFilter<User> filter)
	{
		var user = _userRepository.GetAllByFilter(filter).FirstOrDefault();

		_validationHelper.CheckIfNotNull(user);

		return user;
	}

	private void SendEmail(InitPasswordResetModel model, string token)
	{
		var message = new MailMessage(
			_optionManager.CurrentValue.SmtpSettings.Sender,
			model.Email,
			Resource.MessageSubject_PasswordReset,
			string.Format(Resource.MessageTemplate_PasswordReset, token))
		{
			IsBodyHtml = true
		};

		_messageService.SendEmail(message);
	}
}