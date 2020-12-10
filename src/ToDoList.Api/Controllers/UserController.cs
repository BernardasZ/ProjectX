using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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

        [HttpGet]
        [Route("Users")]
        [Authorize(Policy = CheckPermissions)]
        [SessionCheck]
        public ActionResult<IEnumerable<UserModel>> GetUserList()
        {
            return Ok(userService.GetUserList());
        }

        [HttpPost]
        [Route("Create")]
        [AllowAnonymous]
        public IActionResult CreateUser([FromBody] UserCreateModel model)
        {
            var data = new UserModel()
            {
                UserEmail = model.UserEmail,
                UserName = model.UserName,
                Password = model.Password
            };

            userService.CreateUser(data);

            return Ok();
        }

        [HttpGet]
        [Route("{userId}")]
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
        public ActionResult<UserModel> UpdateUser([FromBody] UserUpdateModel model)
        {
            var data = new UserModel()
            {
                UserId = model.UserId,
                UserEmail = model.UserEmail,
                UserName = model.UserName,
            };

            return Ok(userService.UpdateUser(data));
        }

        [HttpDelete]
        [Route("Delete")]
        [Authorize(Policy = CheckPermissions)]
        [SessionCheck]
        public ActionResult DeleteUser([FromBody] UserDeleteModel model)
        {
            var data = new UserModel() { UserId = model.UserId };
            userService.DeleteUser(data);
            return Ok();
        }
    }
}
