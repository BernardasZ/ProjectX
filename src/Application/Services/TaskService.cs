using Application.Filters;
using Application.Services.Interfaces;
using Application.Validations;
using Domain.Abstractions;
using Domain.Filters;
using Domain.Models;

namespace Application.Services;

public class TaskService : ITaskService
{
	private readonly IRepositoryBase<TaskModel> _taskRepository;
	private readonly IRepositoryBase<UserModel> _userRepository;
	private readonly IUserValidationService _userValidation;
	private readonly ITaskValidationService _taskValidation;

	public TaskService(
		IRepositoryBase<TaskModel> taskRepository,
		IRepositoryBase<UserModel> userRepository,
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

		item.User = _userRepository.GetById(item.User.Id.Value);

		return _taskRepository.Insert(item);
	}

	public TaskModel GetById(int id)
	{
		var task = _taskRepository.GetById(id);

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

		task.Title = item.Title;
		task.Description = item.Description;
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

	public List<TaskModel> GetAll(IFilter<TaskModel> filter = null) => throw new NotImplementedException();
}