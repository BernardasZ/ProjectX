using Application.Database.Repositories;
using Application.Services.Interfaces;
using Application.Validations;
using Domain.Enums;
using Domain.Exeptions;
using Domain.Filters;
using Domain.Models;

namespace Application.Services;

public class UserService : IServiceBase<UserModel>
{
	private readonly IRepository<UserModel> _userRepository;
	private readonly IRepository<RoleModel> _roleRepository;
	private readonly IUserValidationService _userValidation;

	public UserService(
		IRepository<UserModel> userRepository,
		IRepository<RoleModel> roleRepository,
		IUserValidationService userValidation)
	{
		_userRepository = userRepository;
		_roleRepository = roleRepository;
		_userValidation = userValidation;
	}

	public List<UserModel> GetAll(IFilter<UserModel> filter)
	{
		if (!_userValidation.CheckIfUserIsAdmin())
		{
			throw new GenericException(GenericError.OperationIsUnavailable);
		}

		return _userRepository.GetAll(filter).ToList();
	}

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
		user.UserSessions = null;

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
			throw new GenericException(GenericError.UserExist);
		}
	}
}