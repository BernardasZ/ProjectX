using DataModel.Entities.ProjectX;
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
		private readonly IJwtHelper jwtHelper;
		private readonly IAesCryptoHelper aesCryptoHelper;
		private readonly IClientContextScraper clientContextScraper;
		private readonly IHashCryptoHelper hashCryptoHelper;
		private readonly IOptionsMonitor<OptionManager> optionManager;

		public UserService(
			IRepository<UserData> userDataRepository,
			IUserSessionService userSessionService,
			IJwtHelper jwtHelper,
			IAesCryptoHelper aesCryptoHelper,
			IClientContextScraper clientContextScraper,
			IHashCryptoHelper hashCryptoHelper,
			IOptionsMonitor<OptionManager> optionManager)
		{
			this.userDataRepository = userDataRepository;
			this.userSessionService = userSessionService;
			this.jwtHelper = jwtHelper;
			this.aesCryptoHelper = aesCryptoHelper;
			this.clientContextScraper = clientContextScraper;
			this.hashCryptoHelper = hashCryptoHelper;
			this.optionManager = optionManager;
		}

		public UserLoginResponseModel Login(UserLoginModel model)
		{
			model.Password = hashCryptoHelper.HashString(model.Password);

			var data = userDataRepository.FetchAll()
				.Where(x => x.UserEmail == model.UserEmail && x.PassHash == model.Password)
				.Select(x => new 
				{ 
					user = x, 
					role = x.Role.RoleValue 
				})
				.FirstOrDefault();

			if (data != null)
			{
				userSessionService.DeleteUserSession(data.user.Id.ToString());
				userSessionService.CreateUserSession(data.user.Id.ToString());

				if (data.user.FailedLoginCount != 0)
				{
					data.user.FailedLoginCount = 0;
					userDataRepository.Update(data.user);
					userDataRepository.Save();
				}
			}
			else
			{
				throw new GenericException(Enums.GenericErrorEnum.UserDoesNotExist);
			}

			var claims = new ClaimsIdentity(new Claim[]
			{
				new Claim(ClaimTypes.Name, aesCryptoHelper.EncryptString(data.user.Id.ToString())),
				new Claim(ClaimTypes.Role, data.role.ToString())
			});

			string jwt = jwtHelper.ConstructUserJwt(claims);

			return  new UserLoginResponseModel()
			{
				UserName = data.user.UserName,
				UserEmail = data.user.UserEmail,
				UserId = data.user.Id,
				Role = data.role,
				JWT = jwt
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

			if (userData != null)
			{
				userData.PassHash = model.NewPassword;
				userData.FailedLoginCount = 0;
				userDataRepository.Update(userData);
				userDataRepository.Save();
			}
			else
			{
				throw new GenericException(Enums.GenericErrorEnum.UserDoesNotExist);
			}
		}

		public void ResetPassword(UserResetPasswordModel model)
		{
			model.NewPassword = hashCryptoHelper.HashString(model.NewPassword);

			var userData = userDataRepository.FetchAll().Where(x => x.TokenHash == model.Token && x.IsTokenUsed == false && x.TokenExpirationTime.Value >= DateTime.Now).FirstOrDefault();

			if (userData != null)
			{
				userData.PassHash = model.NewPassword;
				userData.IsTokenUsed = true;
				userData.FailedLoginCount = 0;

				userDataRepository.Update(userData);
				userDataRepository.Save();
			}
			else
			{
				throw new GenericException(Enums.GenericErrorEnum.UserDoesNotExist);
			}
		}

		public void InitUserPasswordReset(UserModel model)
		{
			var userData = userDataRepository.FetchAll().Where(x => x.UserEmail == model.UserEmail).FirstOrDefault();

			if (userData != null)
			{
				string token = hashCryptoHelper.HashString(model.UserEmail);
				int expirationMins = optionManager.CurrentValue.AppSettings.PasswordResetExpirationInMin;

				userData.IsTokenUsed = false;
				userData.TokenExpirationTime = DateTime.Now.AddMinutes(expirationMins);
				userData.TokenHash = token;
				userDataRepository.Update(userData);
				userDataRepository.Save();

				//SendEmail
			}
			else
			{
				throw new GenericException(Enums.GenericErrorEnum.UserDoesNotExist);
			}
		}

		public bool CheckIfUserExists(UserModel model)
		{
			return userDataRepository.FetchAll().Where(x => (x.UserEmail == model.UserEmail)).Any();
		}
	}
}
