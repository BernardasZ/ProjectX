using DataModel.Repositories;
using System.Collections.Generic;
using System.Linq;
using ToDoList.Api.Helpers;
using ToDoList.Api.Models.Task;
using TaskData = DataModel.Entities.ProjectX.Task;

namespace ToDoList.Api.Services.Concrete;

public class TaskService : ITaskService
{
	private readonly IRepository<TaskData> _taskRepository;
	private readonly IUserServiceValidationHelper _userServiceValidationHelper;
	private readonly ITaskServiceValidationHelper _taskServiceValidationHelper;

	public TaskService(
		IRepository<TaskData> taskRepository,
		IUserServiceValidationHelper userServiceValidationHelper,
		ITaskServiceValidationHelper taskServiceValidationHelper)
	{
		_taskRepository = taskRepository;
		_userServiceValidationHelper = userServiceValidationHelper;
		_taskServiceValidationHelper = taskServiceValidationHelper;
	}

	public List<TaskModel> GetTaskList(TaskModel model)
	{
		if (!_userServiceValidationHelper.IsAdmin())
		{
			_userServiceValidationHelper.ValidateUserId(model.UserId);
		}

		return _taskRepository
			.FetchAll()
			.Where(x => x.UserId == model.UserId)
			.Select(x => new TaskModel() 
			{ 
				Id = x.Id, 
				UserId = x.UserId, 
				TaskName = x.TaskName, 
				Status = x.Status 
			})
			.ToList();
	}

	public TaskModel CreateTask(TaskModel model)
	{
		_userServiceValidationHelper.ValidateUserId(model.UserId);

		var taskData = new TaskData()
		{
			TaskName = model.TaskName,
			UserId = model.UserId,
			Status = model.Status
		};

		_taskRepository.Insert(taskData);
		_taskRepository.Save();

		model.Id = taskData.Id;

		return model;
	}

	public TaskModel ReadTask(TaskModel model)
	{
		bool isAdmin = _userServiceValidationHelper.IsAdmin();
		var taskData = _taskRepository.GetById(model.Id);

		_taskServiceValidationHelper.ValidateTaskData(taskData);

		if (!isAdmin)
		{
			_userServiceValidationHelper.ValidateUserId(taskData.UserId);
		}

		return new TaskModel()
		{
			Id = taskData.Id,
			UserId = taskData.UserId,
			TaskName = taskData.TaskName,
			Status = taskData.Status
		};
	}

	public TaskModel UpdateTask(TaskModel model)
	{
		var taskData = _taskRepository.GetById(model.Id);

		_taskServiceValidationHelper.ValidateTaskData(taskData);
		_userServiceValidationHelper.ValidateUserId(taskData.UserId);

		taskData.TaskName = model.TaskName;
		taskData.Status = model.Status;

		_taskRepository.Update(taskData);
		_taskRepository.Save();

		model.Id = taskData.Id;

		return model;
	}

	public void DeleteTask(TaskModel model)
	{
		bool isAdmin = _userServiceValidationHelper.IsAdmin();
		var taskData = _taskRepository
			.FetchAll()
			.FirstOrDefault(x => (x.UserId == model.UserId || isAdmin == true) && x.Id == model.Id);

		_taskServiceValidationHelper.ValidateTaskData(taskData);

		if (!isAdmin)
		{
			_userServiceValidationHelper.ValidateUserId(taskData.UserId);
		}

		_taskRepository.Delete(taskData);
		_taskRepository.Save();
	}
}
