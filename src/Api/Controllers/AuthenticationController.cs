using Api.Attributes;
using Api.Constants;
using Api.Models.Login;
using Api.Models.User;
using Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("authentication")]
[ApiController]
public class AuthenticationController : ControllerBase
{
	private readonly IUserLoginService _userLoginService;
	private readonly IUserRecoverService _userRecoverService;

	public AuthenticationController(
		IUserLoginService userLoginService,
		IUserRecoverService userRecoverService)
	{
		_userLoginService = userLoginService;
		_userRecoverService = userRecoverService;
	}

	[HttpPost]
	[Route("login")]
	[AllowAnonymous]
	public ActionResult<UserLoginResponseModel> Login([FromBody] UserLoginModel model) =>
		Ok(_userLoginService.Login(model));

	[HttpPost]
	[Route("logout")]
	[Authorize(Permissions.CheckPermissions)]
	[SessionCheck]
	public ActionResult Logout()
	{
		_userLoginService.Logout();

		return Ok();
	}

	[HttpPost]
	[Route("change-password")]
	[Authorize(Permissions.CheckPermissions)]
	[SessionCheck]
	public ActionResult<UserModel> ChangePassword([FromBody] UserChangePasswordModel model) =>
		Ok(_userRecoverService.ChangePassword(model));

	[HttpPost]
	[Route("reset-password")]
	[AllowAnonymous]
	public ActionResult<UserModel> ResetPassword([FromBody] UserResetPasswordModel model) =>
		Ok(_userRecoverService.ResetPassword(model));

	[HttpPost]
	[Route("init-password-reset")]
	[AllowAnonymous]
	public ActionResult<bool> InitPasswordReset([FromBody] InitPasswordResetModel model) =>
		Ok(_userRecoverService.InitUserPasswordReset(model));
}