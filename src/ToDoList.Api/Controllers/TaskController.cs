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
        private readonly ITaskService taskService;

        public TaskController(
            IOptionsMonitor<OptionManager> optionsManager,
            ITaskService taskService)
        {
            this.optionsManager = optionsManager;
            this.taskService = taskService;
        }

        [HttpGet]
        [Route("Tasks")]
        [Authorize(Policy = CheckPermissions)]
        public ActionResult<IEnumerable<TaskModel>> Tasks([FromQuery] int userId)
        {
            var model = new TaskModel() { UserId = userId };
            var tasks = taskService.GetTaskList(model);

            return Ok(tasks);
        }

        [HttpPut]
        [Route("Create")]
        [Authorize(Policy = CheckPermissions)]
        public IActionResult CreateTask([FromBody] TaskModel model)
        {
            var task = taskService.CreateTask(model);

            return Ok(task);
        }

        [HttpGet]
        [Route("Read")]
        [Authorize(Policy = CheckPermissions)]
        public ActionResult<TaskModel> ReadTask([FromQuery] int taskId)
        {
            var model = new TaskModel() { Id = taskId };
            var task = taskService.ReadTask(model);

            return Ok(task);
        }

        [HttpPost]
        [Route("Update")]
        [Authorize(Policy = CheckPermissions)]
        public ActionResult<TaskModel> UpdateTask([FromBody] TaskModel model)
        {
            var task = taskService.UpdateTask(model);

            return Ok(task);
        }

        [HttpDelete]
        [Route("Delete")]
        [Authorize(Policy = CheckPermissions)]
        public ActionResult DeleteTask([FromBody] TaskModel model)
        {
            taskService.DeleteTask(model);

            return Ok();
        }
    }
}
