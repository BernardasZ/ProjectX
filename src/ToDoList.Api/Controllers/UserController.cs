using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using ToDoList.Api.Exeptions;
using ToDoList.Api.Helpers;
using ToDoList.Api.Models;
using ToDoList.Api.Services;

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
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            try
            {
                model.Password = hashCryptoHelper.HashString(model.Password);

                var user = mapper.Map<UserModel>(model);
                var userData = userService.Login(user);
                var jwt = userService.GetNewJwt(userData);

                return Ok(new { JWT = jwt });
            }
            catch (GenericException e)
			{
                return StatusCode(StatusCodes.Status500InternalServerError, new { ErrorCode = e.ErrorCode.ToString() });
            }
            catch (Exception e)
			{
                return StatusCode(StatusCodes.Status500InternalServerError);
            }      
        }
    }
}
