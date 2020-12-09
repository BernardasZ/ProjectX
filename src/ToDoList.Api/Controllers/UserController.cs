using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ToDoList.Api.Attributes;
using ToDoList.Api.Models.User;
using ToDoList.Api.Services;
using static ToDoList.Api.Constants.Permissions;

namespace ToDoList.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(
            IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost]
        [Route("Create")]
        [AllowAnonymous]
        public IActionResult CreateUser([FromBody] UserModel model)
        {
            userService.CreateUser(model);

            return Ok();
        }

        [HttpGet]
        [Route("User/{userId}")]
        [Authorize(Policy = CheckPermissions)]
        [SessionCheck]
        public ActionResult<UserModel> ReadUser(int userId)
        {
            var model = new UserModel() { UserId = userId };

            return Ok(userService.ReadUser(model));
        }

        [HttpPatch]
        [Route("Update")]
        [Authorize(Policy = CheckPermissions)]
        [SessionCheck]
        public ActionResult<UserModel> UpdateUser([FromBody] UserModel model)
        {
            return Ok(userService.UpdateUser(model));
        }

        [HttpDelete]
        [Route("Delete")]
        [Authorize(Policy = CheckPermissions)]
        [SessionCheck]
        public ActionResult DeleteUser([FromBody] UserModel model)
        {
            userService.DeleteUser(model);
            return Ok();
        }

        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public ActionResult Login([FromBody] UserLoginModel model)
        {
            return Ok(userService.Login(model));
        }

        [HttpPost]
        [Route("Logout")]
        [Authorize(Policy = CheckPermissions)]
        [SessionCheck]
        public ActionResult Logout()
        {
            userService.Logout();

            return Ok();
        }

        [HttpPatch]
        [Route("ChangePassword")]
        [Authorize(Policy = CheckPermissions)]
        [SessionCheck]
        public ActionResult ChangePassword([FromBody] UserChangePasswordModel model)
        {
            userService.ChangePassword(model);

            return Ok();
        }

        [HttpPatch]
        [Route("ResetPassword")]
        [AllowAnonymous]
        public ActionResult ResetPassword([FromBody] UserResetPasswordModel model)
        {
            userService.ResetPassword(model);

            return Ok();
        }

        [HttpPost]
        [Route("InitPasswordReset")]
        [AllowAnonymous]
        public async Task<IActionResult> InitPasswordReset([FromBody] InitPasswordResetModel model)
        {
            userService.InitUserPasswordReset(model);

            return Ok();
        }
    }
}
