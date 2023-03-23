using Api.Attributes;
using Api.DTOs.Task;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Constants;
using Domain.Models;
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
	private readonly IMapper _mapper;

	public TasksController(
		ITaskService taskService,
		IMapper mapper)
	{
		_taskService = taskService;
		_mapper = mapper;
	}

	[HttpGet("user/{id}")]
	public ActionResult<IEnumerable<TaskDto>> GetAllByUserId(int id)
	{
		var result = _taskService.GetAllTasksByUserId(id);

		return Ok(_mapper.Map<List<TaskDto>>(result));
	}

	[HttpPost]
	public ActionResult<TaskDto> Create([FromBody] TaskCreateDto dto)
	{
		var task = _mapper.Map<TaskModel>(dto);

		var result = _taskService.Create(task);

		return Ok(_mapper.Map<TaskDto>(result));
	}

	[HttpGet("{id}")]
	public ActionResult<TaskDto> GetById(int id)
	{
		var result = _taskService.GetById(id);

		return Ok(_mapper.Map<TaskDto>(result));
	}

	[HttpPut]
	public ActionResult<TaskDto> Update([FromBody] TaskUpdateDto dto)
	{
		var task = _mapper.Map<TaskModel>(dto);

		var result = _taskService.Update(task);

		return Ok(_mapper.Map<TaskDto>(result));
	}

	[HttpDelete]
	public ActionResult Delete([FromBody] TaskDeleteDto dto)
	{
		var task = _mapper.Map<TaskModel>(dto);

		_taskService.Delete(task);

		return Ok();
	}
}