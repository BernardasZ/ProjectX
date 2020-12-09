using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ToDoList.Api.Attributes;
using ToDoList.Api.Models.Task;
using ToDoList.Api.Services;
using static ToDoList.Api.Constants.Permissions;

namespace ToDoList.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [SessionCheck]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService taskService;

        public TaskController(
            ITaskService taskService)
        {
            this.taskService = taskService;
        }

        [HttpGet]
        [Route("Tasks/{userId}")]
        [Authorize(Policy = CheckPermissions)]
        public ActionResult<IEnumerable<TaskModel>> Tasks(int userId)
        {
            var model = new TaskModel() { UserId = userId };

            return Ok(taskService.GetTaskList(model));
        }

        [HttpPost]
        [Route("Create")]
        [Authorize(Policy = CheckPermissions)]
        public IActionResult CreateTask([FromBody] TaskModel model)
        {
            return Ok(taskService.CreateTask(model));
        }

        [HttpGet]
        [Route("Task/{taskId}")]
        [Authorize(Policy = CheckPermissions)]
        public ActionResult<TaskModel> ReadTask(int taskId)
        {
            var model = new TaskModel() { Id = taskId };

            return Ok(taskService.ReadTask(model));
        }

        [HttpPatch]
        [Route("Update")]
        [Authorize(Policy = CheckPermissions)]
        public ActionResult<TaskModel> UpdateTask([FromBody] TaskModel model)
        {
            return Ok(taskService.UpdateTask(model));
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
