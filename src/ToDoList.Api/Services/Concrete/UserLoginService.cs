using DataModel.Entities.ProjectX;
using DataModel.Repositories;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using ToDoList.Api.Exeptions;
using ToDoList.Api.Helpers;
using ToDoList.Api.Models.Login;

namespace ToDoList.Api.Services.Concrete
{
	public class UserLoginService : IUserLoginService
	{
		private readonly IRepository<UserData> userDataRepository;
		private readonly IUserSessionService userSessionService;
		private readonly IJwtHelper jwtHelper;
		private readonly IAesCryptoHelper aesCryptoHelper;
		private readonly IClientContextScraper clientContextScraper;
		private readonly IHashCryptoHelper hashCryptoHelper;
		private readonly IOptionsMonitor<OptionManager> optionManager;
		private readonly IMessageService messageService;
		private readonly IUserServiceValidationHelper userServiceValidationHelper;

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
			this.userDataRepository = userDataRepository;
			this.userSessionService = userSessionService;
			this.jwtHelper = jwtHelper;
			this.aesCryptoHelper = aesCryptoHelper;
			this.clientContextScraper = clientContextScraper;
			this.hashCryptoHelper = hashCryptoHelper;
			this.optionManager = optionManager;
			this.messageService = messageService;
			this.userServiceValidationHelper = userServiceValidationHelper;
		}

		public UserLoginResponseModel Login(UserLoginModel model)
		{
			model.Password = hashCryptoHelper.HashString(model.Password);

			var data = userDataRepository.FetchAll()
				.Where(x => x.UserEmail == model.UserEmail && x.PassHash == model.Password)
				.Select(x => new
				{
					x,
					x.Role.RoleValue
				})
				.FirstOrDefault();

			if (data == null)
				throw new GenericException(Enums.GenericErrorEnum.UserDoesNotExist);

			var userData = data.x;
			var role = data.RoleValue;

			userSessionService.DeleteUserSession(userData.Id.ToString());
			userSessionService.CreateUserSession(userData.Id.ToString());

			if (userData.FailedLoginCount != 0)
			{
				userData.FailedLoginCount = 0;
				userDataRepository.Update(userData);
				userDataRepository.Save();
			}

			var claims = new ClaimsIdentity(new Claim[]
			{
				new Claim(ClaimTypes.Name, aesCryptoHelper.EncryptString(userData.Id.ToString())),
				new Claim(ClaimTypes.Role, role.ToString())
			});

			return new UserLoginResponseModel()
			{
				JWT = jwtHelper.ConstructUserJwt(claims)
			};
		}

		public void Logout()
		{
			userSessionService.DeleteUserSession();
		}

		public void ChangePassword(UserChangePasswordModel model)
		{
			model.NewPassword = hashCryptoHelper.HashString(model.NewPassword);
			model.OldPassword = hashCryptoHelper.HashString(model.OldPassword);

			var userId = int.Parse(aesCryptoHelper.DecryptString(clientContextScraper.GetClientClaimsIdentityName()));
			var userData = userDataRepository.FetchAll().Where(x => x.Id == userId && x.UserEmail == model.UserEmail && x.PassHash == model.OldPassword).FirstOrDefault();

			userServiceValidationHelper.ValidateUserData(userData);

			userData.PassHash = model.NewPassword;
			userData.FailedLoginCount = 0;
			userDataRepository.Update(userData);
			userDataRepository.Save();
		}

		public void ResetPassword(UserResetPasswordModel model)
		{
			model.NewPassword = hashCryptoHelper.HashString(model.NewPassword);

			var userData = userDataRepository.FetchAll().Where(x => x.TokenHash == model.Token).FirstOrDefault();

			userServiceValidationHelper.ValidateUserData(userData);
			userServiceValidationHelper.ValidateUserPasswordToken(userData);

			userData.PassHash = model.NewPassword;
			userData.IsTokenUsed = true;
			userData.FailedLoginCount = 0;

			userDataRepository.Update(userData);
			userDataRepository.Save();
		}

		public void InitUserPasswordReset(InitPasswordResetModel model)
		{
			var userData = userDataRepository.FetchAll().Where(x => x.UserEmail == model.UserEmail).FirstOrDefault();

			userServiceValidationHelper.ValidateUserData(userData);

			string token = hashCryptoHelper.HashString(model.UserEmail);
			int expirationMins = optionManager.CurrentValue.AppSettings.PasswordResetExpirationInMin;

			userData.IsTokenUsed = false;
			userData.TokenExpirationTime = DateTime.Now.AddMinutes(expirationMins);
			userData.TokenHash = token;
			userDataRepository.Update(userData);
			userDataRepository.Save();

			string from = optionManager.CurrentValue.SmtpSettings.Sender;
			string to = model.UserEmail;
			string subject = Resource.MessageSubject_PasswordReset;
			string body = string.Format(Resource.MessageTemplate_PasswordReset, token);

			var message = new MailMessage(from, to, subject, body);
			message.IsBodyHtml = true;

			messageService.SendEmail(message);
		}
	}
}
