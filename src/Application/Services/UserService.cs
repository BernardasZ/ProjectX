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

	public async Task<List<UserModel>> GetAllAsync(IFilter<UserModel> filter) => !_userValidation.CheckIfUserIsAdmin()
			? throw new ValidationException(ValidationErrorCodes.UserIdentityMissMatch)
			: await _userRepository.GetAllAsync(filter);

	public async Task<UserModel> CreateAsync(UserModel item)
	{
		await CheckIfUserHaveDuplicatesAsync(item);

		var roleFilter = new RoleFilter { Value = UserRole.User };

		var role = (await _roleRepository.GetAllAsync(roleFilter)).FirstOrDefault();

		item.Role = role;

		return await _userRepository.InsertAsync(item);
	}

	public async Task<UserModel> GetByIdAsync(int id)
	{
		if (!_userValidation.CheckIfUserIsAdmin())
		{
			_userValidation.CheckIfUserIdMatchesSessionId(id);
		}

		return await _userRepository.GetByIdAsync(id);
	}

	public async Task<UserModel> UpdateAsync(UserModel item)
	{
		if (!_userValidation.CheckIfUserIsAdmin())
		{
			_userValidation.CheckIfUserIdMatchesSessionId(item.Id.Value);
		}

		await CheckIfUserHaveDuplicatesAsync(item);

		var user = await _userRepository.GetByIdAsync(item.Id.Value);

		user.Email = item.Email;
		user.Name = item.Name;

		return await _userRepository.UpdateAsync(user);
	}

	public async Task DeleteAsync(UserModel item)
	{
		if (!_userValidation.CheckIfUserIsAdmin())
		{
			_userValidation.CheckIfUserIdMatchesSessionId(item.Id.Value);
		}

		var user = await _userRepository.GetByIdAsync(item.Id.Value);

		await _userSessionService.DeleteAllUserSessionsAsync(user.Id.Value);
		await _userRepository.DeleteAsync(user);
	}

	private async Task CheckIfUserHaveDuplicatesAsync(UserModel item)
	{
		var filter = new FindAnyUserFilter
		{
			Id = item.Id.Value,
			Email = item.Email,
			Name = item.Name
		};

		if ((await _userRepository.GetAllAsync(filter)).Any())
		{
			throw new ValidationException(ValidationErrorCodes.UserExist);
		}
	}
}