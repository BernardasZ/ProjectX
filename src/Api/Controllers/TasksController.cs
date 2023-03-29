using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Attributes;
using Api.Constants;
using Api.DTOs.Task;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
	public async Task<ActionResult<IEnumerable<TaskDto>>> GetAllByUserIdAsync(int id)
	{
		var result = await _taskService.GetAllTasksByUserIdAsync(id);

		return Ok(_mapper.Map<List<TaskDto>>(result));
	}

	[HttpPost]
	public async Task<ActionResult<TaskDto>> CreateAsync([FromBody] TaskCreateDto dto)
	{
		var task = _mapper.Map<TaskModel>(dto);

		var result = await _taskService.CreateAsync(task);

		return Ok(_mapper.Map<TaskDto>(result));
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<TaskDto>> GetByIdAsync(int id)
	{
		var result = await _taskService.GetByIdAsync(id);

		return Ok(_mapper.Map<TaskDto>(result));
	}

	[HttpPut]
	public async Task<ActionResult<TaskDto>> UpdateAsync([FromBody] TaskUpdateDto dto)
	{
		var task = _mapper.Map<TaskModel>(dto);

		var result = await _taskService.UpdateAsync(task);

		return Ok(_mapper.Map<TaskDto>(result));
	}

	[HttpDelete]
	public async Task<ActionResult> DeleteAsync([FromBody] TaskDeleteDto dto)
	{
		var task = _mapper.Map<TaskModel>(dto);

		await _taskService.DeleteAsync(task);

		return Ok();
	}
}