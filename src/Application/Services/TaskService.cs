﻿using Application.Database.Repositories;
using Application.Services.Interfaces;
using Application.Validations;
using Domain.Filters;
using Domain.Models;

namespace Application.Services;

public class TaskService : ITaskService
{
	private readonly IRepository<TaskModel> _taskRepository;
	private readonly IRepository<UserModel> _userRepository;
	private readonly IUserValidationService _userValidation;
	private readonly ITaskValidationService _taskValidation;

	public TaskService(
		IRepository<TaskModel> taskRepository,
		IRepository<UserModel> userRepository,
		IUserValidationService userValidation,
		ITaskValidationService taskValidation)
	{
		_taskRepository = taskRepository;
		_userRepository = userRepository;
		_userValidation = userValidation;
		_taskValidation = taskValidation;
	}

	public List<TaskModel> GetAllTasksByUserId(int id)
	{
		if (!_userValidation.CheckIfUserIsAdmin())
		{
			_userValidation.CheckIfUserIdMatchesSessionId(id);
		}

		return _taskRepository.GetAll(new TaskFilter { UserId = id });
	}

	public TaskModel Create(TaskModel item)
	{
		_userValidation.CheckIfUserIdMatchesSessionId(item.User.Id.Value);

		var user = _userRepository.GetById(item.User.Id.Value);

		_userValidation.CheckIfUserNotNull(user);

		item.User = user;

		return _taskRepository.Insert(item);
	}

	public TaskModel GetById(int id)
	{
		var task = _taskRepository.GetById(id);

		_taskValidation.CheckIfTaskNotNull(task);

		if (!_userValidation.CheckIfUserIsAdmin())
		{
			_userValidation.CheckIfUserIdMatchesSessionId(task.User.Id.Value);
		}

		return task;
	}

	public TaskModel Update(TaskModel item)
	{
		_userValidation.CheckIfUserIdMatchesSessionId(item.User.Id.Value);

		var filter = new TaskFilter
		{
			Id = item.Id,
			UserId = item.User.Id
		};

		var task = _taskRepository.GetAll(filter).FirstOrDefault();

		_taskValidation.CheckIfTaskNotNull(task);

		task.Name = item.Name;
		task.Status = item.Status;

		return _taskRepository.Update(task);
	}

	public void Delete(TaskModel item)
	{
		_userValidation.CheckIfUserIdMatchesSessionId(item.User.Id.Value);

		var filter = new TaskFilter
		{
			Id = item.Id,
			UserId = _userValidation.CheckIfUserIsAdmin() ? null : item.User.Id
		};

		var task = _taskRepository.GetAll(filter).FirstOrDefault();

		_taskValidation.CheckIfTaskNotNull(task);
		_taskRepository.Delete(task);
	}

	public List<TaskModel> GetAll(IFilter<TaskModel> filter = null)
	{
		throw new NotImplementedException();
	}
}