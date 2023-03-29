using Application.Filters;
using Application.Helpers.Cryptography;
using Application.Messages;
using Application.Models.Login;
using Application.Services.Interfaces;
using Application.Services.Options;
using Application.Validations;
using Domain.Abstractions;
using Domain.Filters;
using Domain.Models;
using Microsoft.Extensions.Options;

namespace Application.Services;

public class UserRecoverService : IUserRecoverService
{
	private readonly IRepositoryBase<UserModel> _userRepository;
	private readonly IHashCryptoHelper _cryptoHelper;
	private readonly IOptionsMonitor<UserSettings> _appSettings;
	private readonly IMessageService _messageService;
	private readonly IUserValidationService _userValidation;
	private readonly IDateTime _dateTime;

	public UserRecoverService(
		IRepositoryBase<UserModel> userDataRepository,
		IHashCryptoHelper hashCryptoHelper,
		IOptionsMonitor<UserSettings> appSettings,
		IMessageService messageService,
		IUserValidationService userValidation,
		IDateTime dateTime)
	{
		_userRepository = userDataRepository;
		_cryptoHelper = hashCryptoHelper;
		_appSettings = appSettings;
		_messageService = messageService;
		_userValidation = userValidation;
		_dateTime = dateTime;
	}

	public async Task<UserModel> ChangePasswordAsync(UserChangePasswordModel model)
	{
		var filter = new UserFilter
		{
			Email = model.Email,
			PassHash = model.OldPassHash
		};

		var user = await GetUserByFilterAsync(filter);

		user.PassHash = model.NewPassHash;
		user.FailedLoginCount = 0;

		return await _userRepository.UpdateAsync(user);
	}

	public async Task<UserModel> ResetPasswordAsync(UserResetPasswordModel model)
	{
		var user = await GetUserByFilterAsync(new UserFilter { TokenHash = model.Token });

		_userValidation.CheckIfUserPasswordResetTokenIsValid(user);

		user.PassHash = model.NewPassHash;
		user.IsTokenUsed = true;
		user.FailedLoginCount = 0;

		return await _userRepository.UpdateAsync(user);
	}

	public async Task InitUserPasswordResetAsync(InitPasswordResetModel model)
	{
		var user = await GetUserByFilterAsync(new UserFilter { Email = model.Email });

		user.IsTokenUsed = false;
		user.TokenExpirationTime = _dateTime.GetDateTime()
			.AddMinutes(_appSettings.CurrentValue.PasswordResetExpirationInMin);
		user.TokenHash = _cryptoHelper.GetHashString(model.Email);

		var result = await _userRepository.UpdateAsync(user);

		SendPasswordResetConfitmationEmail(result);
	}

	private async Task<UserModel> GetUserByFilterAsync(IFilter<UserModel> filter)
	{
		var user = (await _userRepository.GetAllAsync(filter)).FirstOrDefault();

		_userValidation.CheckIfUserNotNull(user);

		return user;
	}

	private void SendPasswordResetConfitmationEmail(UserModel model) =>
		_messageService.SendEmailMessage(
			model.Email,
			ApplicationResource.MessageSubject_PasswordReset,
			string.Format(ApplicationResource.MessageTemplate_PasswordReset, model.TokenHash));
}