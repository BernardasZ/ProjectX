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
        public IActionResult CreateTask([FromBody] TaskCreateModel model)
        {
            var data = new TaskModel() 
            { 
                UserId = model.UserId,
                TaskName = model.TaskName,
                Status = model.Status
            };

            return Ok(taskService.CreateTask(data));
        }

        [HttpGet]
        [Route("{taskId}")]
        [Authorize(Policy = CheckPermissions)]
        public ActionResult<TaskModel> ReadTask(int taskId)
        {
            var model = new TaskModel() { Id = taskId };

            return Ok(taskService.ReadTask(model));
        }

        [HttpPatch]
        [Route("Update")]
        [Authorize(Policy = CheckPermissions)]
        public ActionResult<TaskModel> UpdateTask([FromBody] TaskUpdateModel model)
        {
            var data = new TaskModel()
            {
                Id = model.Id,
                TaskName = model.TaskName,
                Status = model.Status
            };

            return Ok(taskService.UpdateTask(data));
        }

        [HttpDelete]
        [Route("Delete")]
        [Authorize(Policy = CheckPermissions)]
        public ActionResult DeleteTask([FromBody] TaskDeleteModel model)
        {
            var data = new TaskModel()
            {
                Id = model.Id,
                UserId = model.UserId
            };

            taskService.DeleteTask(data);

            return Ok();
        }
    }
}
