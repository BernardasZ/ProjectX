using System;
using System.Collections.Generic;
using System.Linq;
using DataModel.DbContexts;
using DataModel.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;
using ToDoList.Api.Helpers;
using ToDoList.Api.Models;

namespace ToDoList.Api.Controllers
{
	[Route("api/[controller]")]
    [ApiController]
    public class ToDoTasksController : ControllerBase
    {
        private readonly IOptionsMonitor<OptionsManager> options;

        public ToDoTasksController(
            IOptionsMonitor<OptionsManager> options)
        {
            this.options = options;
        }

        [HttpGet]
        [Route("GetToDoTaskList")]
        public ActionResult<IEnumerable<ToDoTask>> GetToDoTaskList([FromQuery] int userId)
        {
            return new JsonResult(new List<Models.ToDoTask>() { new ToDoTask() { Id = 1 } });
        }

        [HttpPost]
        [Route("CreateToDoTask")]
        public IActionResult CreateToDoTask([FromBody] ToDoTask toDoTask)
        {
            return Ok();
        }

        [HttpGet]
        [Route("ReadToDoTask")]
        public ActionResult<ToDoTask> ReadToDoTask([FromQuery] int taskId)
        {
            return new JsonResult(new ToDoTask());
        }

        [HttpPost]
        [Route("UpdateToDoTask")]
        public ActionResult<ToDoTask> UpdateToDoTask([FromBody,] ToDoTask toDoTask)
        {
            return new JsonResult(toDoTask);
        }

        [HttpPost]
        [Route("DeleteToDoTask")]
        public ActionResult<ToDoTask> DeleteToDoTask([FromBody] int taskId)
        {
            return Ok();
        }
    }
}
