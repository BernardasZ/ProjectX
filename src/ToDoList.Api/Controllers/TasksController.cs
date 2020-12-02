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
using ToDoList.Api.Authorization;
using ToDoList.Api.Constants;
using ToDoList.Api.Helpers;
using ToDoList.Api.Models;
using ToDoList.Api.Services;
using static ToDoList.Api.Constants.Constants.UserPermissions;

namespace ToDoList.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly IOptionsMonitor<OptionManager> optionsManager;

        public TasksController(
            IOptionsMonitor<OptionManager> optionsManager)
        {
            this.optionsManager = optionsManager;
        }

        [HttpGet]
        [Route("GetTasksList")]
        [Authorize(Policy = Read)]
        public ActionResult<IEnumerable<ToDoTask>> GetTasksList([FromQuery] int userId)
        {
            return new JsonResult(new List<Models.ToDoTask>() { new ToDoTask() { Id = 1 } });
        }

        [HttpPost]
        [Route("CreateTask")]
        [Authorize(Policy = Create)]
        public IActionResult CreateTask([FromBody] ToDoTask toDoTask)
        {
            return Ok();
        }

        [HttpGet]
        [Route("ReadTask")]
        [Authorize(Policy = Read)]
        public ActionResult<ToDoTask> ReadTask([FromQuery] int taskId)
        {
            return new JsonResult(new ToDoTask());
        }

        [HttpPost]
        [Route("UpdateTask")]
        [Authorize(Policy = Update)]
        public ActionResult<ToDoTask> UpdateTask([FromBody] ToDoTask toDoTask)
        {
            return new JsonResult(toDoTask);
        }

        [HttpPost]
        [Route("DeleteTask")]
        [Authorize(Policy = Delete)]
        public ActionResult<ToDoTask> DeleteTask([FromBody] int taskId)
        {
            return Ok();
        }
    }
}
