using DataModel.Entities.ProjectX;
using DataModel.Enums;
using DataModel.Repositories;
using System.Collections.Generic;
using System.Linq;
using ToDoList.Api.Exeptions;
using ToDoList.Api.Helpers;
using ToDoList.Api.Models.User;

namespace ToDoList.Api.Services.Concrete;

public class UserService : IUserService
{
	private readonly IRepository<UserData> _userDataRepository;
	private readonly IRepository<Role> _roleRepository;
	private readonly IHashCryptoHelper _hashCryptoHelper;
	private readonly IUserServiceValidationHelper _userServiceValidationHelper;
	private readonly IUserLoginService _userLoginService;

	public UserService(
		IRepository<UserData> userDataRepository,
		IRepository<Role> roleRepository,
		IHashCryptoHelper hashCryptoHelper,
		IUserServiceValidationHelper userServiceValidationHelper,
		IUserLoginService userLoginService)
	{
		_userDataRepository = userDataRepository;
		_roleRepository = roleRepository;
		_hashCryptoHelper = hashCryptoHelper;
		_userServiceValidationHelper = userServiceValidationHelper;
		_userLoginService = userLoginService;
	}

	public IEnumerable<UserModel> GetUserList()
	{
		if (!_userServiceValidationHelper.IsAdmin())
		{
			throw new GenericException(Enums.GenericErrorEnum.OperationIsUnavailable);
		}

		return _userDataRepository.
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
		if (_userDataRepository.FetchAll().Any(x => x.UserName == model.UserName || x.UserEmail == model.UserEmail))
		{
			throw new GenericException(Enums.GenericErrorEnum.UserExist);
		}

		var roleId = _roleRepository.FetchAll().Where(x => x.RoleValue == UserRoleEnum.User).Select(x => x.Id).FirstOrDefault();

		model.Password = _hashCryptoHelper.HashString(model.Password);

		var userData = new UserData()
		{
			UserName = model.UserName,
			UserEmail = model.UserEmail,
			PassHash = model.Password,
			RoleId = roleId
		};

		_userDataRepository.Insert(userData);
		_userDataRepository.Save();
	}

	public UserModel ReadUser(UserModel model)
	{
		var data = _userDataRepository.GetById(model.UserId);

		_userServiceValidationHelper.ValidateUserData(data);
		_userServiceValidationHelper.ValidateUserId(model.UserId);

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
		var needUpdate = false;
		var userData = _userDataRepository.GetById(model.UserId);

		_userServiceValidationHelper.ValidateUserData(userData);
		_userServiceValidationHelper.ValidateUserId(model.UserId);

		if (model.UserName != userData.UserName && _userDataRepository.FetchAll().Any(x => x.UserName == model.UserName))
		{
			throw new GenericException(Enums.GenericErrorEnum.UserExist);
		}
		else
		{
			userData.UserName = model.UserName;
			needUpdate = true;
		}
		
		if (model.UserEmail != userData.UserEmail && _userDataRepository.FetchAll().Any(x => x.UserEmail == model.UserEmail))
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
			_userDataRepository.Update(userData);
			_userDataRepository.Save();
		}

		return model;
	}

	public void DeleteUser(UserModel model)
	{
		var data = _userDataRepository.GetById(model.UserId);

		_userServiceValidationHelper.ValidateUserData(data);
		_userServiceValidationHelper.ValidateUserId(model.UserId);
		_userLoginService.Logout();

		_userDataRepository.Delete(data);
		_userDataRepository.Save();
	}
}
