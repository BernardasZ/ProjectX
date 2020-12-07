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
using ToDoList.Api.Models;

namespace ToDoList.Api.Services.Concrete
{
	public class UserService : IUserService
	{
		private readonly IRepository<UserData> userDataRepository;
		private readonly IUserSessionService userSessionService;
		private readonly IJwtHelper jwtHelper;
		private readonly IAesCryptoHelper aesCryptoHelper;
		private readonly IClientContextScraper clientContextScraper;


		public UserService(
			IRepository<UserData> userDataRepository,
			IUserSessionService userSessionService,
			IJwtHelper jwtHelper,
			IAesCryptoHelper aesCryptoHelper,
			IClientContextScraper clientContextScraper)
		{
			this.userDataRepository = userDataRepository;
			this.userSessionService = userSessionService;
			this.jwtHelper = jwtHelper;
			this.aesCryptoHelper = aesCryptoHelper;
			this.clientContextScraper = clientContextScraper;
		}

		public UserModel Login(UserModel user)
		{
			var userData = GetUserData(user)
				.Select(x => new UserModel
				{
					UserId = x.Id,
					Role = x.Role.RoleValue,
					UserName = x.UserName,
					UserEmail = x.UserEmail,
					Password = x.PassHash
				})
				.FirstOrDefault();

			if (userData != null)
			{
				userSessionService.DeleteUserSession(userData.UserId.ToString());
				userSessionService.CreateUserSession(userData.UserId.ToString());
			}
			else
			{
				throw new GenericException(Enums.GenericErrorEnum.UserDoesNotExist);
			}

			return userData;
		}

		public void LoginOut(UserModel user)
		{
			if (CheckIfUserExists(user))
			{
				userSessionService.DeleteUserSession();
			}
			else
			{
				throw new GenericException(Enums.GenericErrorEnum.UserDoesNotExist);
			}
		}

		public void ChangePassword(UserPassChangeModel user)
		{
			var userData = GetUserData(user).FirstOrDefault();

			if (userData != null)
			{
				userData.PassHash = user.NewPassword;
				userDataRepository.Update(userData);
				userDataRepository.Save();
			}
			else
			{
				throw new GenericException(Enums.GenericErrorEnum.UserDoesNotExist);
			}
		}

		public bool CheckIfUserExists(UserModel user)
		{
			return userDataRepository.FetchAll().Where(x => (x.UserName == user.UserName || x.UserEmail == user.UserName)).Any();
		}

		public IQueryable<UserData> GetUserData(UserModel user)
		{
			return userDataRepository
				.FetchAll()
				.Where(x => (x.UserName == user.UserName || x.UserEmail == user.UserEmail) && x.PassHash == user.Password);
		}

		public string GetNewJwt(UserModel user)
		{
			string userIdentity = aesCryptoHelper.EncryptString(user.UserId.ToString());

			var claims = new ClaimsIdentity(new Claim[]
			{
				new Claim(ClaimTypes.Name, userIdentity),
				new Claim(ClaimTypes.Role, user.Role.ToString())
			});

			return jwtHelper.ConstructUserJwt(claims);
		}

		public bool CheckUserIdentity(string id)
		{
			string userIdentity = aesCryptoHelper.DecryptString(clientContextScraper.GetClientClaimsName());

			if (userIdentity == id)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}
