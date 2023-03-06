using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Api.Attributes;
using ToDoList.Api.Models.Login;
using ToDoList.Api.Services;
using static ToDoList.Api.Constants.Permissions;

namespace ToDoList.Api.Controllers
{
	[Route("api/authentication")]
	[ApiController]
	public class AuthenticationController : ControllerBase
	{
        private readonly IUserLoginService userLoginService;
		public AuthenticationController(IUserLoginService userLoginService)
		{
            this.userLoginService = userLoginService;
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public ActionResult Login([FromBody] UserLoginModel model)
        {
            return Ok(userLoginService.Login(model));
        }

        [HttpPost]
        [Route("logout")]
        [Authorize(CheckPermissions)]
        [SessionCheck]
        public ActionResult Logout()
        {
            userLoginService.Logout();

            return Ok();
        }

        [HttpPost]
        [Route("change-password")]
        [Authorize(CheckPermissions)]
        [SessionCheck]
        public ActionResult ChangePassword([FromBody] UserChangePasswordModel model)
        {
            userLoginService.ChangePassword(model);

            return Ok();
        }

        [HttpPost]
        [Route("reset-password")]
        [AllowAnonymous]
        public ActionResult ResetPassword([FromBody] UserResetPasswordModel model)
        {
            userLoginService.ResetPassword(model);

            return Ok();
        }

        [HttpPost]
        [Route("init-password-reset")]
        [AllowAnonymous]
        public IActionResult InitPasswordReset([FromBody] InitPasswordResetModel model)
        {
            userLoginService.InitUserPasswordReset(model);

            return Ok();
        }
    }
}
