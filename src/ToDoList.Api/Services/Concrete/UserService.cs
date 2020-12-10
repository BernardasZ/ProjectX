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
		private readonly IRepository<Role> roleRepository;
		private readonly IHashCryptoHelper hashCryptoHelper;
		private readonly IUserServiceValidationHelper userServiceValidationHelper;
		private readonly IUserLoginService userLoginService;

		public UserService(
			IRepository<UserData> userDataRepository,
			IRepository<Role> roleRepository,
			IHashCryptoHelper hashCryptoHelper,
			IUserServiceValidationHelper userServiceValidationHelper,
			IUserLoginService userLoginService)
		{
			this.userDataRepository = userDataRepository;
			this.roleRepository = roleRepository;
			this.hashCryptoHelper = hashCryptoHelper;
			this.userServiceValidationHelper = userServiceValidationHelper;
			this.userLoginService = userLoginService;
		}

		public IEnumerable<UserModel> GetUserList()
		{
			if (!userServiceValidationHelper.IsAdmin())
				throw new GenericException(Enums.GenericErrorEnum.OperationIsUnavailable);

			return userDataRepository.
				FetchAll()
				.Select(x => new UserModel
				{
					UserId = x.Id,
					UserName = x.UserName,
					UserEmail = x.UserEmail,
					Role = x.Role.RoleValue,
				})
				.ToList();
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

			userServiceValidationHelper.ValidateUserData(data);
			userServiceValidationHelper.ValidateUserId(model.UserId);

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

			userServiceValidationHelper.ValidateUserData(userData);
			userServiceValidationHelper.ValidateUserId(model.UserId);

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

			userServiceValidationHelper.ValidateUserData(data);
			userServiceValidationHelper.ValidateUserId(model.UserId);
			userLoginService.Logout();

			userDataRepository.Delete(data);
			userDataRepository.Save();
		}
	}
}
