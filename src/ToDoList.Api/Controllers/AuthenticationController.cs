using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Api.Attributes;
using ToDoList.Api.Models.Login;
using ToDoList.Api.Services;
using static ToDoList.Api.Constants.Permissions;

namespace ToDoList.Api.Controllers;

[Route("api/authentication")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IUserLoginService _userLoginService;
	public AuthenticationController(IUserLoginService userLoginService)
	{
        _userLoginService = userLoginService;
    }

    [HttpPost]
    [Route("login")]
    [AllowAnonymous]
    public ActionResult Login([FromBody] UserLoginModel model)
    {
        return Ok(_userLoginService.Login(model));
    }

    [HttpPost]
    [Route("logout")]
    [Authorize(CheckPermissions)]
    [SessionCheck]
    public ActionResult Logout()
    {
        _userLoginService.Logout();

        return Ok();
    }

    [HttpPost]
    [Route("change-password")]
    [Authorize(CheckPermissions)]
    [SessionCheck]
    public ActionResult ChangePassword([FromBody] UserChangePasswordModel model)
    {
        _userLoginService.ChangePassword(model);

        return Ok();
    }

    [HttpPost]
    [Route("reset-password")]
    [AllowAnonymous]
    public ActionResult ResetPassword([FromBody] UserResetPasswordModel model)
    {
        _userLoginService.ResetPassword(model);

        return Ok();
    }

    [HttpPost]
    [Route("init-password-reset")]
    [AllowAnonymous]
    public IActionResult InitPasswordReset([FromBody] InitPasswordResetModel model)
    {
        _userLoginService.InitUserPasswordReset(model);

        return Ok();
    }
}
