using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using ToDoList.Api.Exeptions;
using ToDoList.Api.Helpers;
using ToDoList.Api.Models.User;
using ToDoList.Api.Services;
using static ToDoList.Api.Constants.Permissions;

namespace ToDoList.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IOptionsMonitor<OptionManager> optionsManager;
        private readonly IUserService userService;
        private readonly IHashCryptoHelper hashCryptoHelper;
        private readonly IMapper mapper;

        public UserController(
            IOptionsMonitor<OptionManager> optionsManager,
            IUserService userService,
            IHashCryptoHelper hashCryptoHelper,
            IMapper mapper)
        {
            this.optionsManager = optionsManager;
            this.userService = userService;
            this.hashCryptoHelper = hashCryptoHelper;
            this.mapper = mapper;
        }

        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserLoginModel model)
        {
            var data = userService.Login(model);

            return Ok(data);
        }

        [HttpPost]
        [Route("Logout")]
        [Authorize(Policy = CheckPermissions)]
        public async Task<IActionResult> Logout()
        {
            userService.Logout();

            return Ok();
        }

        [HttpPost]
        [Route("ChangePassword")]
        [Authorize(Policy = CheckPermissions)]
        public async Task<IActionResult> ChangePassword([FromBody] UserChangePasswordModel model)
        {
            userService.ChangePassword(model);

            return Ok();
        }

        [HttpPost]
        [Route("ResetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] UserResetPasswordModel model)
        {
            userService.ResetPassword(model);

            return Ok();
        }

        [HttpPost]
        [Route("InitPasswordReset")]
        [AllowAnonymous]
        public async Task<IActionResult> InitPasswordReset([FromBody] UserModel model)
        {
            userService.InitUserPasswordReset(model);

            return Ok();
        }
    }
}
