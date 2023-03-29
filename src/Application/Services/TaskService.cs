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

	public async Task<List<TaskModel>> GetAllTasksByUserIdAsync(int id)
	{
		if (!_userValidation.CheckIfUserIsAdmin())
		{
			_userValidation.CheckIfUserIdMatchesSessionId(id);
		}

		return await _taskRepository.GetAllAsync(new TaskFilter { UserId = id });
	}

	public async Task<TaskModel> CreateAsync(TaskModel item)
	{
		_userValidation.CheckIfUserIdMatchesSessionId(item.User.Id.Value);

		item.User = await _userRepository.GetByIdAsync(item.User.Id.Value);

		return await _taskRepository.InsertAsync(item);
	}

	public async Task<TaskModel> GetByIdAsync(int id)
	{
		var task = await _taskRepository.GetByIdAsync(id);

		if (!_userValidation.CheckIfUserIsAdmin())
		{
			_userValidation.CheckIfUserIdMatchesSessionId(task.User.Id.Value);
		}

		return task;
	}

	public async Task<TaskModel> UpdateAsync(TaskModel item)
	{
		_userValidation.CheckIfUserIdMatchesSessionId(item.User.Id.Value);

		var filter = new TaskFilter
		{
			Id = item.Id,
			UserId = item.User.Id
		};

		var task = (await _taskRepository.GetAllAsync(filter)).FirstOrDefault();

		_taskValidation.CheckIfTaskNotNull(task);

		task.Title = item.Title;
		task.Description = item.Description;
		task.Status = item.Status;

		return await _taskRepository.UpdateAsync(task);
	}

	public async Task DeleteAsync(TaskModel item)
	{
		_userValidation.CheckIfUserIdMatchesSessionId(item.User.Id.Value);

		var filter = new TaskFilter
		{
			Id = item.Id,
			UserId = _userValidation.CheckIfUserIsAdmin() ? null : item.User.Id
		};

		var task = (await _taskRepository.GetAllAsync(filter)).FirstOrDefault();

		_taskValidation.CheckIfTaskNotNull(task);
		await _taskRepository.DeleteAsync(task);
	}

	public Task<List<TaskModel>> GetAllAsync(IFilter<TaskModel> filter = null) => Task.FromResult<List<TaskModel>>(new());
}