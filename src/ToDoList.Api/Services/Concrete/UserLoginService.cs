using DataModel.Entities.ProjectX;
using DataModel.Repositories;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using ToDoList.Api.Exeptions;
using ToDoList.Api.Helpers;
using ToDoList.Api.Models.Login;

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
		model.Password = _hashCryptoHelper.HashString(model.Password);

		var data = _userDataRepository.FetchAll()
			.Where(x => x.UserEmail == model.UserEmail && x.PassHash == model.Password)
			.Select(x => new
			{
				x,
				x.Role.RoleValue
			})
			.FirstOrDefault();

		if (data == null)
		{
			throw new GenericException(Enums.GenericErrorEnum.UserDoesNotExist);
		}

		var userData = data.x;
		var role = data.RoleValue;

		_userSessionService.DeleteUserSession(userData.Id.ToString());
		_userSessionService.CreateUserSession(userData.Id.ToString());

		if (userData.FailedLoginCount != 0)
		{
			userData.FailedLoginCount = 0;
			_userDataRepository.Update(userData);
			_userDataRepository.Save();
		}

		var claims = new ClaimsIdentity(new Claim[]
		{
			new Claim(ClaimTypes.Name, _aesCryptoHelper.EncryptString(userData.Id.ToString())),
			new Claim(ClaimTypes.Role, role.ToString())
		});

		return new UserLoginResponseModel()
		{
			JWT = _jwtHelper.ConstructUserJwt(claims)
		};
	}

	public void Logout()
	{
		_userSessionService.DeleteUserSession();
	}

	public void ChangePassword(UserChangePasswordModel model)
	{
		model.NewPassword = _hashCryptoHelper.HashString(model.NewPassword);
		model.OldPassword = _hashCryptoHelper.HashString(model.OldPassword);

		var userId = int.Parse(_aesCryptoHelper.DecryptString(_clientContextScraper.GetClientClaimsIdentityName()));
		var userData = _userDataRepository
			.FetchAll()
			.FirstOrDefault(x => x.Id == userId && x.UserEmail == model.UserEmail && x.PassHash == model.OldPassword);

		_userServiceValidationHelper.ValidateUserData(userData);

		userData.PassHash = model.NewPassword;
		userData.FailedLoginCount = 0;
		_userDataRepository.Update(userData);
		_userDataRepository.Save();
	}

	public void ResetPassword(UserResetPasswordModel model)
	{
		model.NewPassword = _hashCryptoHelper.HashString(model.NewPassword);

		var userData = _userDataRepository
			.FetchAll()
			.FirstOrDefault(x => x.TokenHash == model.Token);

		_userServiceValidationHelper.ValidateUserData(userData);
		_userServiceValidationHelper.ValidateUserPasswordToken(userData);

		userData.PassHash = model.NewPassword;
		userData.IsTokenUsed = true;
		userData.FailedLoginCount = 0;

		_userDataRepository.Update(userData);
		_userDataRepository.Save();
	}

	public void InitUserPasswordReset(InitPasswordResetModel model)
	{
		var userData = _userDataRepository
			.FetchAll()
			.FirstOrDefault(x => x.UserEmail == model.UserEmail);

		_userServiceValidationHelper.ValidateUserData(userData);

		var token = _hashCryptoHelper.HashString(model.UserEmail);
		var expirationMins = _optionManager.CurrentValue.AppSettings.PasswordResetExpirationInMin;

		userData.IsTokenUsed = false;
		userData.TokenExpirationTime = DateTime.Now.AddMinutes(expirationMins);
		userData.TokenHash = token;

		_userDataRepository.Update(userData);
		_userDataRepository.Save();

		var from = _optionManager.CurrentValue.SmtpSettings.Sender;
		var to = model.UserEmail;
		var subject = Resource.MessageSubject_PasswordReset;
		var body = string.Format(Resource.MessageTemplate_PasswordReset, token);

		var message = new MailMessage(from, to, subject, body);
		message.IsBodyHtml = true;

		_messageService.SendEmail(message);
	}
}
