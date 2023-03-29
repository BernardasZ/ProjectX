using Application.Authentication;
using Application.Exceptions;
using Application.Exceptions.Enums;
using Application.Filters;
using Application.Models.Login;
using Application.Services.Interfaces;
using Application.Validations;
using AutoMapper;
using Domain.Abstractions;
using Domain.Models;

namespace Application.Services;

public class UserLoginService : IUserLoginService
{
	private readonly IRepositoryBase<UserModel> _userRepository;
	private readonly IUserSessionService _userSessionService;
	private readonly IJwtService _jwtHelper;
	private readonly IUserValidationService _userValidation;
	private readonly IMapper _mapper;

	public UserLoginService(
		IRepositoryBase<UserModel> userDataRepository,
		IUserSessionService userSessionService,
		IJwtService jwtHelper,
		IUserValidationService userValidation,
		IMapper mapper)
	{
		_userRepository = userDataRepository;
		_userSessionService = userSessionService;
		_jwtHelper = jwtHelper;
		_userValidation = userValidation;
		_mapper = mapper;
	}

	public async Task<UserLoginResponseModel> LoginAsync(UserLoginModel model)
	{
		var filter = new UserFilter { Email = model.Email, };

		var user = (await _userRepository.GetAllAsync(filter)).FirstOrDefault();

		await CheckUserLoginValidationCriteriaAsync(model, user);

		await _userSessionService.DeleteUserSessionsByIpAndUserIdAsync(user.Id.Value);
		var session = await _userSessionService.CreateUserSessionAsync(user.Id.ToString());

		user.FailedLoginCount = 0;
		user.UserSessions.Add(session);
		user = await _userRepository.UpdateAsync(user);

		var jwt = _jwtHelper.ConstructUserJwt(user.Role.Value.ToString(), session.SessionIdentifier);

		return _mapper.Map<UserModel, UserLoginResponseModel>(user, map =>
			map.AfterMap((src, dest) => dest.JWT = jwt));
	}

	private async Task CheckUserLoginValidationCriteriaAsync(UserLoginModel model, UserModel user)
	{
		_userValidation.CheckIfUserNotNull(user);

		if (user.FailedLoginCount > 10)
		{
			throw new ValidationException(ValidationErrorCodes.UserIsBlocked);
		}

		if (user.PassHash != model.PassHash)
		{
			user.FailedLoginCount++;
			await _userRepository.UpdateAsync(user);

			throw new ValidationException(ValidationErrorCodes.UserPasswordIsIncorrect);
		}
	}

	public async Task LogoutAsync() => await _userSessionService.DeleteUserSessionsByIpAndUserIdAsync();
}