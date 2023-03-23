using Application.Authentication;
using Application.Database.Repositories;
using Application.Models.Login;
using Application.Services.Interfaces;
using Application.Validations;
using AutoMapper;
using Domain.Enums;
using Domain.Exeptions;
using Domain.Filters;
using Domain.Models;

namespace Application.Services;

public class UserLoginService : IUserLoginService
{
	private readonly IRepository<UserModel> _userRepository;
	private readonly IUserSessionService _userSessionService;
	private readonly IJwtService _jwtHelper;
	private readonly IUserValidationService _userValidation;
	private readonly IMapper _mapper;

	public UserLoginService(
		IRepository<UserModel> userDataRepository,
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

	public UserLoginResponseModel Login(UserLoginModel model)
	{
		var filter = new UserFilter { Email = model.Email, };

		var user = _userRepository.GetAll(filter).FirstOrDefault();

		CheckUserLoginValidationCriteria(model, user);

		_userSessionService.DeleteUserSessionsByIpAndUserId(user.Id.Value);

		user.FailedLoginCount = 0;
		user = _userRepository.Update(user);

		var session = _userSessionService.CreateUserSession(user.Id.ToString());

		var jwt = _jwtHelper.ConstructUserJwt(user.Role.Value.ToString(), session.SessionIdentifier);

		return _mapper.Map<UserModel, UserLoginResponseModel>(user, map =>
			map.AfterMap((src, dest) => dest.JWT = jwt));
	}

	private void CheckUserLoginValidationCriteria(UserLoginModel model, UserModel user)
	{
		_userValidation.CheckIfUserNotNull(user);

		if (user.PassHash != model.PassHash)
		{
			user.FailedLoginCount++;
			_userRepository.Update(user);

			throw new GenericException(GenericError.UserPasswordIsIncorrect);
		}

		if (user.PassHash == model.PassHash && user.FailedLoginCount > 10)
		{
			throw new GenericException(GenericError.UserIsBlocked);
		}
	}

	public void Logout() => _userSessionService.DeleteUserSessionsByIpAndUserId();
}