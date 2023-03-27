using Application.Exceptions;
using Application.Exceptions.Enums;
using Application.Filters;
using Application.Services.Interfaces;
using Application.Validations;
using Domain.Abstractions;
using Domain.Enums;
using Domain.Filters;
using Domain.Models;

namespace Application.Services;

public class UserService : IServiceBase<UserModel>
{
	private readonly IRepositoryBase<UserModel> _userRepository;
	private readonly IRepositoryBase<RoleModel> _roleRepository;
	private readonly IUserValidationService _userValidation;
	private readonly IUserSessionService _userSessionService;

	public UserService(
		IRepositoryBase<UserModel> userRepository,
		IRepositoryBase<RoleModel> roleRepository,
		IUserValidationService userValidation,
		IUserSessionService userSessionService)
	{
		_userRepository = userRepository;
		_roleRepository = roleRepository;
		_userValidation = userValidation;
		_userSessionService = userSessionService;
	}

	public List<UserModel> GetAll(IFilter<UserModel> filter) => !_userValidation.CheckIfUserIsAdmin()
			? throw new ValidationException(ValidationErrorCodes.UserIdentityMissMatch)
			: _userRepository.GetAll(filter).ToList();

	public UserModel Create(UserModel item)
	{
		CheckIfUserHaveDuplicates(item);

		var roleFilter = new RoleFilter { Value = UserRole.User };

		var role = _roleRepository.GetAll(roleFilter).FirstOrDefault();

		item.Role = role;

		return _userRepository.Insert(item);
	}

	public UserModel GetById(int id)
	{
		if (!_userValidation.CheckIfUserIsAdmin())
		{
			_userValidation.CheckIfUserIdMatchesSessionId(id);
		}

		return _userRepository.GetById(id);
	}

	public UserModel Update(UserModel item)
	{
		if (!_userValidation.CheckIfUserIsAdmin())
		{
			_userValidation.CheckIfUserIdMatchesSessionId(item.Id.Value);
		}

		CheckIfUserHaveDuplicates(item);

		var user = _userRepository.GetById(item.Id.Value);

		user.Email = item.Email;
		user.Name = item.Name;

		return _userRepository.Update(user);
	}

	public void Delete(UserModel item)
	{
		if (!_userValidation.CheckIfUserIsAdmin())
		{
			_userValidation.CheckIfUserIdMatchesSessionId(item.Id.Value);
		}

		var user = _userRepository.GetById(item.Id.Value);

		_userSessionService.DeleteAllUserSessions(user.Id.Value);
		_userRepository.Delete(user);
	}

	private void CheckIfUserHaveDuplicates(UserModel item)
	{
		var filter = new FindAnyUserFilter
		{
			Email = item.Email,
			Name = item.Name
		};

		if (_userRepository.GetAll(filter).Any())
		{
			throw new ValidationException(ValidationErrorCodes.UserExist);
		}
	}
}