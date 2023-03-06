using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ToDoList.Api.Attributes;
using ToDoList.Api.Models.Task;
using ToDoList.Api.Services;
using static ToDoList.Api.Constants.Permissions;

namespace ToDoList.Api.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    [SessionCheck]
    [Authorize(CheckPermissions)]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService taskService;

        public TasksController(ITaskService taskService)
        {
            this.taskService = taskService;
        }

        [HttpGet]
        [Route("{userId}")] 
        public ActionResult<IEnumerable<TaskModel>> GetAllByUserId(int userId)
        {
            var model = new TaskModel() { UserId = userId };

            return Ok(taskService.GetTaskList(model));
        }

        [HttpPost]
        public IActionResult Create([FromBody] TaskCreateModel model)
        {
            var data = new TaskModel() 
            { 
                UserId = model.UserId,
                TaskName = model.TaskName,
                Status = model.Status
            };

            return Ok(taskService.CreateTask(data));
        }

        [HttpGet("{id}")]
        public ActionResult<TaskModel> Get(int id)
        {
            var model = new TaskModel() { Id = id };

            return Ok(taskService.ReadTask(model));
        }

        [HttpPut("{id}")]
        public ActionResult<TaskModel> Update([FromBody] TaskUpdateModel model)
        {
            var data = new TaskModel()
            {
                Id = model.Id,
                TaskName = model.TaskName,
                Status = model.Status
            };

            return Ok(taskService.UpdateTask(data));
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromBody] TaskDeleteModel model)
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
