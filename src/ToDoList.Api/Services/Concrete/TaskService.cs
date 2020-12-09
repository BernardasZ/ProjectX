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

		public TaskService(
			IRepository<TaskData> taskRepository,
			IClientContextScraper clientContextScraper,
			IAesCryptoHelper aesCryptoHelper)
		{
			this.taskRepository = taskRepository;
			this.clientContextScraper = clientContextScraper;
			this.aesCryptoHelper = aesCryptoHelper;
		}

		public List<TaskModel> GetTaskList(TaskModel model)
		{
			bool isAdmin = clientContextScraper.GetClientClaimsRole() == UserRoleEnum.Admin.ToString();

			if (!isAdmin) 
				ValidateUserId(model.UserId);

			return taskRepository
				.FetchAll()
				.Where(x => x.UserId == model.UserId || isAdmin == true)
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
			ValidateUserId(model.UserId);

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
			var taskData = taskRepository.GetById(model.Id);

			ValidateUserData(taskData);
			ValidateUserId(taskData.UserId);

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
			ValidateUserId(model.UserId);

			var taskData = taskRepository.GetById(model.Id);

			ValidateUserData(taskData);

			taskData.TaskName = model.TaskName;
			taskData.Status = model.Status;

			taskRepository.Update(taskData);
			taskRepository.Save();

			model.Id = taskData.Id;

			return model;
		}

		public void DeleteTask(TaskModel model)
		{
			bool isAdmin = clientContextScraper.GetClientClaimsRole() == UserRoleEnum.Admin.ToString();

			if (!isAdmin)
				ValidateUserId(model.UserId);

			var taskData = taskRepository.FetchAll().Where(x => (x.UserId == model.UserId || isAdmin == true) && x.Id == model.Id).FirstOrDefault();

			ValidateUserData(taskData);	

			taskRepository.Delete(taskData);
			taskRepository.Save();
		}

		private void ValidateUserData(TaskData model)
		{
			if (model == null)
				throw new GenericException(Enums.GenericErrorEnum.TaskDoesNotExist);
		}

		private void ValidateUserId(int userId)
		{
			if (userId > 0 && userId.ToString() != aesCryptoHelper.DecryptString(clientContextScraper.GetClientClaimsIdentityName()))
				throw new GenericException(Enums.GenericErrorEnum.UserIdentityMissMatch);
		}
	}
}
