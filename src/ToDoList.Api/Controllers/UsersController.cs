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
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        [Authorize(CheckPermissions)]
        [SessionCheck]
        public ActionResult<IEnumerable<UserModel>> GetAll()
        {
            return Ok(userService.GetUserList());
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Create([FromBody] UserCreateModel model)
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

        [HttpGet("{id}")]
        [Authorize(CheckPermissions)]
        [SessionCheck]
        public ActionResult<UserModel> Get(int id)
        {
            var model = new UserModel() { UserId = id };

            return Ok(userService.ReadUser(model));
        }

        [HttpPut("{id}")]
        [Authorize(CheckPermissions)]
        [SessionCheck]
        public ActionResult<UserModel> Update([FromBody] UserUpdateModel model)
        {
            var data = new UserModel()
            {
                UserId = model.UserId,
                UserEmail = model.UserEmail,
                UserName = model.UserName,
            };

            return Ok(userService.UpdateUser(data));
        }

        [HttpDelete("{id}")]
        [Authorize(CheckPermissions)]
        [SessionCheck]
        public ActionResult Delete([FromBody] UserDeleteModel model)
        {
            var data = new UserModel() { UserId = model.UserId };
            userService.DeleteUser(data);
            return Ok();
        }
    }
}
