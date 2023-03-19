using Api.Exeptions;
using Api.Helpers;
using Api.Models.User;
using AutoMapper;
using Persistence.Entities.ProjectX;
using Persistence.Enums;
using Persistence.Filters;
using Persistence.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace Api.Services.Concrete;

public class UserService : IBaseService<UserModel>
{
	private readonly IRepository<User> _userDataRepository;
	private readonly IRepository<Role> _roleRepository;
	private readonly IHashCryptoHelper _hashCryptoHelper;
	private readonly IUserServiceValidationHelper _userServiceValidationHelper;
	private readonly IUserLoginService _userLoginService;
	private readonly IMapper _mapper;

	public UserService(
		IRepository<User> userDataRepository,
		IRepository<Role> roleRepository,
		IHashCryptoHelper hashCryptoHelper,
		IUserServiceValidationHelper userServiceValidationHelper,
		IUserLoginService userLoginService,
		IMapper mapper)
	{
		_userDataRepository = userDataRepository;
		_roleRepository = roleRepository;
		_hashCryptoHelper = hashCryptoHelper;
		_userServiceValidationHelper = userServiceValidationHelper;
		_userLoginService = userLoginService;
		_mapper = mapper;
	}

	public List<UserModel> GetAll()
	{
		if (!_userServiceValidationHelper.CheckIfAdminRole())
		{
			throw new GenericException(Enums.GenericError.OperationIsUnavailable);
		}

		return _userDataRepository.
			GetAllByFilter()
			.Select(x => new UserModel
			{
				Id = x.Id.Value,
				Name = x.Name,
				Email = x.Email,
				Role = x.Role.Value,
			})
			.ToList();
	}

	public UserModel Create(UserModel item)
	{
		CheckUserForDuplicates(item);

		var roleFilter = new RoleEntityFilter { Value = UserRole.User };

		var role = _roleRepository.GetAllByFilter(roleFilter).FirstOrDefault();

		var user = new User
		{
			Name = item.Name,
			Email = item.Email,
			PassHash = _hashCryptoHelper.HashString(item.Password),
			Role = role
		};

		return _mapper.Map<UserModel>(_userDataRepository.Insert(user));
	}

	public UserModel GetById(int id)
	{
		var user = _userDataRepository.GetById(id);

		ValidateUser(id, user);

		return _mapper.Map<UserModel>(user);
	}

	public UserModel Update(UserModel item)
	{
		var user = _userDataRepository.GetById(item.Id);

		ValidateUser(item.Id, user);

		CheckUserForDuplicates(item);

		user.Name = item.Name;
		user.Email = item.Email;

		var userResult = _userDataRepository.Update(user);

		return new UserModel
		{
			Id = userResult.Id.Value,
			Name = userResult.Name,
			Email = userResult.Email,
			Role = userResult.Role.Value,
		};
	}

	public void Delete(UserModel model)
	{
		var data = _userDataRepository.GetById(model.Id);

		ValidateUser(model.Id, data);

		_userLoginService.Logout();
		_userDataRepository.Delete(data);
	}

	private void ValidateUser(int userId, User data)
	{
		_userServiceValidationHelper.CheckIfNotNull(data);
		_userServiceValidationHelper.CheckIfUserIdMatching(userId);
	}

	private void CheckUserForDuplicates(UserModel model)
	{
		var userFilter = new FindAnyUserEntityFilter
		{
			Email = model.Email,
			Name = model.Name
		};

		if (_userDataRepository.GetAllByFilter(userFilter).Any())
		{
			throw new GenericException("User already exists.");
		}
	}
}