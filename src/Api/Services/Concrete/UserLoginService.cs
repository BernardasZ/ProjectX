using Api.Helpers;
using Api.Models.Login;
using Persistence.Entities.ProjectX;
using Persistence.Filters;
using Persistence.Repositories;
using System.Linq;

namespace Api.Services.Concrete;

public class UserLoginService : IUserLoginService
{
	private readonly IRepository<User> _userRepository;
	private readonly IUserSessionService _userSessionService;
	private readonly IJwtHelper _jwtHelper;
	private readonly IHashCryptoHelper _cryptoHelper;
	private readonly IUserServiceValidationHelper _validationHelper;

	public UserLoginService(
		IRepository<User> userDataRepository,
		IUserSessionService userSessionService,
		IJwtHelper jwtHelper,
		IHashCryptoHelper hashCryptoHelper,
		IUserServiceValidationHelper userServiceValidationHelper)
	{
		_userRepository = userDataRepository;
		_userSessionService = userSessionService;
		_jwtHelper = jwtHelper;
		_cryptoHelper = hashCryptoHelper;
		_validationHelper = userServiceValidationHelper;
	}

	public UserLoginResponseModel Login(UserLoginModel model)
	{
		var filter = new UserEntityFilter
		{
			Email = model.Email,
			PassHash = _cryptoHelper.HashString(model.Password)
		};

		var user = _userRepository.GetAllByFilter(filter).FirstOrDefault();

		_validationHelper.CheckIfNotNull(user);

		_userSessionService.DeleteUserSession(user.Id.ToString());
		_userSessionService.CreateUserSession(user.Id.ToString());

		if (user.FailedLoginCount != 0)
		{
			user.FailedLoginCount = 0;
			_userRepository.Update(user);
		}

		return new UserLoginResponseModel()
		{
			JWT = _jwtHelper.ConstructUserJwt(user.Role.Value.ToString(), user.Id.ToString())
		};
	}

	public void Logout() => _userSessionService.DeleteUserSession();
}