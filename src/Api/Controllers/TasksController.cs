using Api.Attributes;
using Api.Constants;
using Api.Models.Task;
using Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Api.Controllers;

[Route("tasks")]
[ApiController]
[SessionCheck]
[Authorize(Permissions.CheckPermissions)]
public class TasksController : ControllerBase
{
	private readonly ITaskService _taskService;

	public TasksController(ITaskService taskService)
	{
		_taskService = taskService;
	}

	[HttpGet("user/{id}")]
	public ActionResult<IEnumerable<TaskModel>> GetAllByUserId(int id) =>
		Ok(_taskService.GetAllTasksByUserId(id));

	[HttpPost]
	public IActionResult Create([FromBody] TaskCreateModel model)
	{
		var data = new TaskModel
		{
			UserId = model.UserId,
			Name = model.Name,
			Status = model.Status
		};

		return Ok(_taskService.Create(data));
	}

	[HttpGet("{id}")]
	public ActionResult<TaskModel> GetById(int id) =>
		Ok(_taskService.GetById(id));

	[HttpPut]
	public ActionResult<TaskModel> Update([FromBody] TaskUpdateModel model)
	{
		var data = new TaskModel
		{
			Id = model.Id,
			UserId = model.UserId,
			Name = model.Name,
			Status = model.Status
		};

		return Ok(_taskService.Update(data));
	}

	[HttpDelete]
	public ActionResult Delete([FromBody] TaskDeleteModel model)
	{
		var data = new TaskModel
		{
			Id = model.Id,
			UserId = model.UserId
		};

		_taskService.Delete(data);

		return Ok();
	}
}