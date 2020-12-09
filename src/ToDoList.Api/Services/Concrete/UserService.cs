using DataModel.Entities.ProjectX;
using DataModel.Enums;
using DataModel.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Api.Exeptions;
using ToDoList.Api.Helpers;
using ToDoList.Api.Models.User;

namespace ToDoList.Api.Services.Concrete
{
	public class UserService : IUserService
	{
		private readonly IRepository<UserData> userDataRepository;
		private readonly IUserSessionService userSessionService;
		private readonly IRepository<Role> roleRepository;
		private readonly IJwtHelper jwtHelper;
		private readonly IAesCryptoHelper aesCryptoHelper;
		private readonly IClientContextScraper clientContextScraper;
		private readonly IHashCryptoHelper hashCryptoHelper;
		private readonly IOptionsMonitor<OptionManager> optionManager;
		private readonly IMessageService messageService;

		public UserService(
			IRepository<UserData> userDataRepository,
			IUserSessionService userSessionService,
			IRepository<Role> roleRepository,
			IJwtHelper jwtHelper,
			IAesCryptoHelper aesCryptoHelper,
			IClientContextScraper clientContextScraper,
			IHashCryptoHelper hashCryptoHelper,
			IOptionsMonitor<OptionManager> optionManager,
			IMessageService messageService)
		{
			this.userDataRepository = userDataRepository;
			this.userSessionService = userSessionService;
			this.roleRepository = roleRepository;
			this.jwtHelper = jwtHelper;
			this.aesCryptoHelper = aesCryptoHelper;
			this.clientContextScraper = clientContextScraper;
			this.hashCryptoHelper = hashCryptoHelper;
			this.optionManager = optionManager;
			this.messageService = messageService;
		}

		public void CreateUser(UserModel model)
		{
			if (userDataRepository.FetchAll().Where(x => x.UserName == model.UserName || x.UserEmail == model.UserEmail).Any())
				throw new GenericException(Enums.GenericErrorEnum.UserExist);

			var roleId = roleRepository.FetchAll().Where(x => x.RoleValue == UserRoleEnum.User).Select(x => x.Id).FirstOrDefault();

			model.Password = hashCryptoHelper.HashString(model.Password);

			var userData = new UserData()
			{
				UserName = model.UserName,
				UserEmail = model.UserEmail,
				PassHash = model.Password,
				RoleId = roleId
			};

			userDataRepository.Insert(userData);
			userDataRepository.Save();
		}

		public UserModel ReadUser(UserModel model)
		{
			var data = userDataRepository.GetById(model.UserId);

			ValidateUserData(data);
			ValidateUserId(model.UserId);

			return new UserModel()
			{
				UserId = data.Id,
				UserName = data.UserName,
				UserEmail = data.UserEmail,
				Role = data.Role.RoleValue,
			};
		}

		public UserModel UpdateUser(UserModel model)
		{
			bool needUpdate = false;
			var userData = userDataRepository.GetById(model.UserId);

			ValidateUserData(userData);
			ValidateUserId(model.UserId);

			if (model.UserName != userData.UserName && userDataRepository.FetchAll().Where(x => x.UserName == model.UserName).Any())
			{
				throw new GenericException(Enums.GenericErrorEnum.UserExist);
			}
			else
			{
				userData.UserName = model.UserName;
				needUpdate = true;
			}
			
			if (model.UserEmail != userData.UserEmail && userDataRepository.FetchAll().Where(x => x.UserEmail == model.UserEmail).Any())
			{
				throw new GenericException(Enums.GenericErrorEnum.UserExist);
			}
			else
			{
				userData.UserEmail = model.UserEmail;
				needUpdate = true;
			}

			if (needUpdate)
			{
				userDataRepository.Update(userData);
				userDataRepository.Save();
			}

			return model;
		}

		public void DeleteUser(UserModel model)
		{
			var data = userDataRepository.GetById(model.UserId);

			ValidateUserData(data);
			ValidateUserId(model.UserId);

			userDataRepository.Delete(data);
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

			var userData = data.x;
			var role = data.RoleValue;

			ValidateUserData(userData);

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

			ValidateUserData(userData);

			userData.PassHash = model.NewPassword;
			userData.FailedLoginCount = 0;
			userDataRepository.Update(userData);
			userDataRepository.Save();
		}

		public void ResetPassword(UserResetPasswordModel model)
		{
			model.NewPassword = hashCryptoHelper.HashString(model.NewPassword);

			var userData = userDataRepository.FetchAll().Where(x => x.TokenHash == model.Token).FirstOrDefault();
			
			ValidateUserData(userData);
			ValidateUserPasswordToken(userData);

			userData.PassHash = model.NewPassword;
			userData.IsTokenUsed = true;
			userData.FailedLoginCount = 0;

			userDataRepository.Update(userData);
			userDataRepository.Save();
		}

		public async void InitUserPasswordReset(InitPasswordResetModel model)
		{
			var userData = userDataRepository.FetchAll().Where(x => x.UserEmail == model.UserEmail).FirstOrDefault();

			ValidateUserData(userData);

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

			messageService.SendEmailAsync(message);
		}

		private void ValidateUserData(UserData model)
		{
			if (model == null) 
				throw new GenericException(Enums.GenericErrorEnum.UserDoesNotExist);
		}

		private void ValidateUserId(int userId)
		{
			if (userId > 0 && userId.ToString() != aesCryptoHelper.DecryptString(clientContextScraper.GetClientClaimsIdentityName())) 
				throw new GenericException(Enums.GenericErrorEnum.UserIdentityMissMatch);
		}

		private void ValidateUserPasswordToken(UserData model)
		{
			if (model.IsTokenUsed == null || model.IsTokenUsed.Value || model.TokenExpirationTime.Value <= DateTime.Now)
				throw new GenericException(Enums.GenericErrorEnum.UserResetPasswordTokenIsExpired);
		}
	}
}
