using DataModel.Entities.ProjectX;
using DataModel.Filters;
using DataModel.Repositories;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Net.Mail;
using ToDoList.Api.Exeptions;
using ToDoList.Api.Helpers;
using ToDoList.Api.Models.Login;
using ToDoList.Api.Models.User;
using ToDoList.Api.Options;

namespace ToDoList.Api.Services.Concrete;

public class UserLoginService : IUserLoginService
{
	private readonly IRepository<UserData> _userDataRepository;
	private readonly IUserSessionService _userSessionService;
	private readonly IJwtHelper _jwtHelper;
	private readonly IAesCryptoHelper _aesCryptoHelper;
	private readonly IClientContextScraper _clientContextScraper;
	private readonly IHashCryptoHelper _hashCryptoHelper;
	private readonly IOptionsMonitor<OptionManager> _optionManager;
	private readonly IMessageService _messageService;
	private readonly IUserServiceValidationHelper _userServiceValidationHelper;

	public UserLoginService(
		IRepository<UserData> userDataRepository,
		IUserSessionService userSessionService,
		IJwtHelper jwtHelper,
		IAesCryptoHelper aesCryptoHelper,
		IClientContextScraper clientContextScraper,
		IHashCryptoHelper hashCryptoHelper,
		IOptionsMonitor<OptionManager> optionManager,
		IMessageService messageService,
		IUserServiceValidationHelper userServiceValidationHelper)
	{
		_userDataRepository = userDataRepository;
		_userSessionService = userSessionService;
		_jwtHelper = jwtHelper;
		_aesCryptoHelper = aesCryptoHelper;
		_clientContextScraper = clientContextScraper;
		_hashCryptoHelper = hashCryptoHelper;
		_optionManager = optionManager;
		_messageService = messageService;
		_userServiceValidationHelper = userServiceValidationHelper;
	}

	public UserLoginResponseModel Login(UserLoginModel model)
	{
		var filter = new UserEntityFilter
		{
			UserEmail = model.UserEmail,
			PassHash = _hashCryptoHelper.HashString(model.Password)
		};

		var user = _userDataRepository.GetAllByFilter(filter).FirstOrDefault();

		if (user == null)
		{
			throw new GenericException(Enums.GenericError.UserDoesNotExist);
		}

		_userSessionService.DeleteUserSession(user.Id.ToString());
		_userSessionService.CreateUserSession(user.Id.ToString());

		if (user.FailedLoginCount != 0)
		{
			user.FailedLoginCount = 0;
			_userDataRepository.Update(user);
		}

		return new UserLoginResponseModel()
		{
			JWT = _jwtHelper.ConstructUserJwt(user.Role.RoleValue.ToString(), user.Id.ToString())
		};
	}

	public void Logout()
	{
		_userSessionService.DeleteUserSession();
	}

	public UserModel ChangePassword(UserChangePasswordModel model)
	{
		var filter = new UserEntityFilter
		{
			Id = int.Parse(_aesCryptoHelper.DecryptString(_clientContextScraper.GetClientClaimsIdentityName())),
			UserEmail = model.UserEmail,
			PassHash = _hashCryptoHelper.HashString(model.OldPassword)
		};

		var user = _userDataRepository.GetAllByFilter(filter).FirstOrDefault();

		_userServiceValidationHelper.ValidateUserData(user);

		user.PassHash = _hashCryptoHelper.HashString(model.NewPassword);
		user.FailedLoginCount = 0;
		var newUser = _userDataRepository.Update(user);
	}

	public void ResetPassword(UserResetPasswordModel model)
	{
		var filter = new UserEntityFilter { TokenHash = model.Token };

		var user = _userDataRepository.GetAllByFilter(filter).FirstOrDefault();

		_userServiceValidationHelper.ValidateUserData(user);
		_userServiceValidationHelper.ValidateUserPasswordToken(user);

		user.PassHash = _hashCryptoHelper.HashString(model.NewPassword);
		user.IsTokenUsed = true;
		user.FailedLoginCount = 0;

		var newUser = _userDataRepository.Update(user);
	}

	public void InitUserPasswordReset(InitPasswordResetModel model)
	{
		var filter = new UserEntityFilter { UserEmail = model.UserEmail };

		var user = _userDataRepository.GetAllByFilter(filter).FirstOrDefault();

		_userServiceValidationHelper.ValidateUserData(user);

		var expirationMins = _optionManager.CurrentValue.AppSettings.PasswordResetExpirationInMin;

		user.IsTokenUsed = false;
		user.TokenExpirationTime = DateTime.Now.AddMinutes(expirationMins);
		user.TokenHash = _hashCryptoHelper.HashString(model.UserEmail);

		_userDataRepository.Update(user);

		SendEmail(model, user.TokenHash);
	}

	private void SendEmail(InitPasswordResetModel model, string token)
	{
		var message = new MailMessage(
			_optionManager.CurrentValue.SmtpSettings.Sender,
			model.UserEmail,
			Resource.MessageSubject_PasswordReset,
			string.Format(Resource.MessageTemplate_PasswordReset, token))
		{
			IsBodyHtml = true
		};

		_messageService.SendEmail(message);
	}
}