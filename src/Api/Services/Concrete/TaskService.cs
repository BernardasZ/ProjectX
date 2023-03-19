using Api.Helpers;
using Api.Models.Task;
using AutoMapper;
using Persistence.Entities.ProjectX;
using Persistence.Filters;
using Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskData = Persistence.Entities.ProjectX.Task;

namespace Api.Services.Concrete;

public class TaskService : ITaskService
{
	private readonly IRepository<TaskData> _taskRepository;
	private readonly IRepository<User> _userRepository;
	private readonly IUserServiceValidationHelper _userValidationHelper;
	private readonly ITaskServiceValidationHelper _taskValidationHelper;
	private readonly IMapper _mapper;

	public TaskService(
		IRepository<TaskData> taskRepository,
		IUserServiceValidationHelper userServiceValidationHelper,
		ITaskServiceValidationHelper taskServiceValidationHelper,
		IMapper mapper,
		IRepository<User> userRepository)
	{
		_taskRepository = taskRepository;
		_userValidationHelper = userServiceValidationHelper;
		_taskValidationHelper = taskServiceValidationHelper;
		_mapper = mapper;
		_userRepository = userRepository;
	}

	public List<TaskModel> GetAll()
	{
		throw new NotImplementedException();
	}

	public List<TaskModel> GetAllTasksByUserId(int id)
	{
		if (!_userValidationHelper.CheckIfAdminRole())
		{
			_userValidationHelper.CheckIfUserIdMatching(id);
		}

		var tasks = _taskRepository.GetAllByFilter(new TaskEntityFilter { UserId = id });

		return _mapper.Map<List<TaskModel>>(tasks);
	}

	public TaskModel Create(TaskModel item)
	{
		_userValidationHelper.CheckIfUserIdMatching(item.UserId);

		var user = _userRepository.GetById(item.UserId);

		var task = _mapper.Map<TaskData>(item);
		task.User = user;

		var taskData = _taskRepository.Insert(task);

		return _mapper.Map<TaskModel>(taskData);
	}

	public TaskModel GetById(int id)
	{
		var task = _taskRepository.GetById(id);

		if (!_userValidationHelper.CheckIfAdminRole())
		{
			_userValidationHelper.CheckIfUserIdMatching(id);
		}

		return _mapper.Map<TaskModel>(task);
	}

	public TaskModel Update(TaskModel item)
	{
		_userValidationHelper.CheckIfUserIdMatching(item.UserId);

		var filter = new TaskEntityFilter
		{
			Id = item.Id,
			UserId = item.UserId
		};

		var task = _taskRepository.GetAllByFilter(filter).FirstOrDefault();

		task.Name = item.Name;
		task.Status = item.Status;

		return _mapper.Map<TaskModel>(_taskRepository.Update(task));
	}

	public void Delete(TaskModel item)
	{
		var isAdmin = _userValidationHelper.CheckIfAdminRole();

		var filter = new TaskEntityFilter
		{
			Id = item.Id,
			UserId = isAdmin ? null : item.UserId
		};

		var task = _taskRepository.GetAllByFilter(filter).FirstOrDefault();

		_taskValidationHelper.CheckIfNotNull(task);

		_taskRepository.Delete(task);
	}
}