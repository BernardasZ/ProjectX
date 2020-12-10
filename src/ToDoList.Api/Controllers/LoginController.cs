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
	[Route("api/[controller]")]
	[ApiController]
	public class LoginController : ControllerBase
	{
        private readonly IUserLoginService userLoginService;
		public LoginController(IUserLoginService userLoginService)
		{
            this.userLoginService = userLoginService;
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login([FromBody] UserLoginModel model)
        {
            return Ok(userLoginService.Login(model));
        }

        [HttpPost]
        [Route("Logout")]
        [Authorize(Policy = CheckPermissions)]
        [SessionCheck]
        public ActionResult Logout()
        {
            userLoginService.Logout();

            return Ok();
        }

        [HttpPatch]
        [Route("ChangePassword")]
        [Authorize(Policy = CheckPermissions)]
        [SessionCheck]
        public ActionResult ChangePassword([FromBody] UserChangePasswordModel model)
        {
            userLoginService.ChangePassword(model);

            return Ok();
        }

        [HttpPatch]
        [Route("ResetPassword")]
        [AllowAnonymous]
        public ActionResult ResetPassword([FromBody] UserResetPasswordModel model)
        {
            userLoginService.ResetPassword(model);

            return Ok();
        }

        [HttpPost]
        [Route("InitPasswordReset")]
        [AllowAnonymous]
        public IActionResult InitPasswordReset([FromBody] InitPasswordResetModel model)
        {
            userLoginService.InitUserPasswordReset(model);

            return Ok();
        }
    }
}
