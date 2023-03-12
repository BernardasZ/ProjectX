using DataModel.Entities.ProjectX;
using DataModel.Enums;
using DataModel.Filters;
using DataModel.Repositories;
using System.Collections.Generic;
using System.Linq;
using ToDoList.Api.Exeptions;
using ToDoList.Api.Helpers;
using ToDoList.Api.Models.User;

namespace ToDoList.Api.Services.Concrete;

public class UserService : IBaseService<UserModel>
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

	public List<UserModel> GetAll()
	{
		if (!_userServiceValidationHelper.IsAdmin())
		{
			throw new GenericException(Enums.GenericError.OperationIsUnavailable);
		}

		return _userDataRepository.
			GetAllByFilter()
			.Select(x => new UserModel
			{
				UserId = x.Id,
				UserName = x.UserName,
				UserEmail = x.UserEmail,
				Role = x.Role.RoleValue,
			})
			.ToList();
	}

	public UserModel Add(UserModel item)
	{
		CheckUserForDuplicates(item);

		var roleFilter = new RoleEntityFilter { RoleValue = UserRoleEnum.User };

		var roleId = _roleRepository.GetAllByFilter(roleFilter).FirstOrDefault();

		var user = new UserData
		{
			UserName = item.UserName,
			UserEmail = item.UserEmail,
			PassHash = _hashCryptoHelper.HashString(item.Password),
			RoleId = roleId.Id
		};

		var userResult = _userDataRepository.Insert(user);

		return new UserModel
		{
			UserId = userResult.Id,
			UserName = userResult.UserName,
			UserEmail = userResult.UserEmail,
			Role = userResult.Role.RoleValue,
		};
	}

	public UserModel GetById(int id)
	{
		var user = _userDataRepository.GetById(id);

		ValidateUser(id, user);

		return new UserModel
		{
			UserId = user.Id,
			UserName = user.UserName,
			UserEmail = user.UserEmail,
			Role = user.Role.RoleValue,
		};
	}

	public UserModel Update(UserModel item)
	{
		var user = _userDataRepository.GetById(item.UserId);

		ValidateUser(item.UserId, user);

		CheckUserForDuplicates(item);

		user.UserName = item.UserName;
		user.UserEmail = item.UserEmail;

		var userResult = _userDataRepository.Update(user);

		return new UserModel
		{
			UserId = userResult.Id,
			UserName = userResult.UserName,
			UserEmail = userResult.UserEmail,
			Role = userResult.Role.RoleValue,
		};
	}

	public void Delete(UserModel model)
	{
		var data = _userDataRepository.GetById(model.UserId);

		ValidateUser(model.UserId, data);

		_userLoginService.Logout();
		_userDataRepository.Delete(data);
	}

	private void ValidateUser(int userId, UserData data)
	{
		_userServiceValidationHelper.ValidateUserData(data);
		_userServiceValidationHelper.ValidateUserId(userId);
	}

	private void CheckUserForDuplicates(UserModel model)
	{
		var userFilter = new FindAnyUserEntityFilter
		{
			UserEmail = model.UserEmail,
			UserName = model.UserName
		};

		if (_userDataRepository.GetAllByFilter(userFilter).Any())
		{
			throw new GenericException("User already exists.");
		}
	}
}