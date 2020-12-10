using DataModel.Enums;
using DataModel.Repositories;
using System.Collections.Generic;
using System.Linq;
using ToDoList.Api.Exeptions;
using ToDoList.Api.Helpers;
using ToDoList.Api.Models.Task;
using TaskData = DataModel.Entities.ProjectX.Task;

namespace ToDoList.Api.Services.Concrete
{
	public class TaskService : ITaskService
	{
		private readonly IRepository<TaskData> taskRepository;
		private readonly IClientContextScraper clientContextScraper;
		private readonly IAesCryptoHelper aesCryptoHelper;
		private readonly IUserServiceValidationHelper userServiceValidationHelper;
		private readonly ITaskServiceValidationHelper taskServiceValidationHelper;

		public TaskService(
			IRepository<TaskData> taskRepository,
			IClientContextScraper clientContextScraper,
			IAesCryptoHelper aesCryptoHelper,
			IUserServiceValidationHelper userServiceValidationHelper,
			ITaskServiceValidationHelper taskServiceValidationHelper)
		{
			this.taskRepository = taskRepository;
			this.clientContextScraper = clientContextScraper;
			this.aesCryptoHelper = aesCryptoHelper;
			this.userServiceValidationHelper = userServiceValidationHelper;
			this.taskServiceValidationHelper = taskServiceValidationHelper;
		}

		public List<TaskModel> GetTaskList(TaskModel model)
		{
			if (!userServiceValidationHelper.IsAdmin())
				userServiceValidationHelper.ValidateUserId(model.UserId);

			return taskRepository
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
			userServiceValidationHelper.ValidateUserId(model.UserId);

			var taskData = new TaskData()
			{
				TaskName = model.TaskName,
				UserId = model.UserId,
				Status = model.Status
			};

			taskRepository.Insert(taskData);
			taskRepository.Save();

			model.Id = taskData.Id;

			return model;
		}

		public TaskModel ReadTask(TaskModel model)
		{
			bool isAdmin = userServiceValidationHelper.IsAdmin();
			var taskData = taskRepository.GetById(model.Id);

			taskServiceValidationHelper.ValidateTaskData(taskData);

			if (!isAdmin)
				userServiceValidationHelper.ValidateUserId(taskData.UserId);

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
			var taskData = taskRepository.GetById(model.Id);

			taskServiceValidationHelper.ValidateTaskData(taskData);
			userServiceValidationHelper.ValidateUserId(taskData.UserId);

			taskData.TaskName = model.TaskName;
			taskData.Status = model.Status;

			taskRepository.Update(taskData);
			taskRepository.Save();

			model.Id = taskData.Id;

			return model;
		}

		public void DeleteTask(TaskModel model)
		{
			bool isAdmin = userServiceValidationHelper.IsAdmin();
			var taskData = taskRepository.FetchAll().Where(x => (x.UserId == model.UserId || isAdmin == true) && x.Id == model.Id).FirstOrDefault();

			taskServiceValidationHelper.ValidateTaskData(taskData);

			if (!isAdmin)
				userServiceValidationHelper.ValidateUserId(taskData.UserId);

			taskRepository.Delete(taskData);
			taskRepository.Save();
		}
	}
}
