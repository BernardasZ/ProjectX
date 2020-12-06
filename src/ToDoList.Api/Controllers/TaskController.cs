using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using DataModel.DbContexts;
using DataModel.Enums;
using DataModel.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;
using ToDoList.Api.Attributes;
using ToDoList.Api.Authorization;
using ToDoList.Api.Constants;
using ToDoList.Api.Helpers;
using ToDoList.Api.Models;
using ToDoList.Api.Services;
using static ToDoList.Api.Constants.Permissions;

namespace ToDoList.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [SessionCheck]
    public class TaskController : ControllerBase
    {
        private readonly IOptionsMonitor<OptionManager> optionsManager;

        public TaskController(
            IOptionsMonitor<OptionManager> optionsManager)
        {
            this.optionsManager = optionsManager;
        }

        [HttpGet]
        [Route("Tasks")]
        [Authorize(Policy = CheckPermissions)]
        public ActionResult<IEnumerable<TaskDTO>> Tasks([FromQuery] int userId)
        {
            return Ok(new List<Models.TaskDTO>() { new TaskDTO() { Id = 1 } });
        }

        [HttpPost]
        [Route("Create")]
        [Authorize(Policy = CheckPermissions)]
        public IActionResult CreateTask([FromBody] TaskDTO toDoTask)
        {
            return Ok();
        }

        [HttpGet]
        [Route("Read")]
        [Authorize(Policy = CheckPermissions)]
        public ActionResult<TaskDTO> ReadTask([FromQuery] int taskId)
        {
            return Ok(new TaskDTO());
        }

        [HttpPost]
        [Route("Update")]
        [Authorize(Policy = CheckPermissions)]
        public ActionResult<TaskDTO> UpdateTask([FromBody] TaskDTO toDoTask)
        {
            return Ok(toDoTask);
        }

        [HttpPost]
        [Route("Delete")]
        [Authorize(Policy = CheckPermissions)]
        public ActionResult<TaskDTO> DeleteTask([FromBody] TaskDTO toDoTask)
        {
            return Ok();
        }
    }
}
