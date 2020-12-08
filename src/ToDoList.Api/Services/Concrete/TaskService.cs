using AutoMapper;
using DataModel.Enums;
using DataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using ToDoList.Api.Exeptions;
using ToDoList.Api.Helpers;
using ToDoList.Api.Models.Task;

namespace ToDoList.Api.Services.Concrete
{
	public class TaskService : ITaskService
	{
		private readonly IRepository<DataModel.Entities.ProjectX.Task> taskRepository;
		private readonly IClientContextScraper clientContextScraper;
		private readonly IMapper mapper;

		public TaskService(
			IRepository<DataModel.Entities.ProjectX.Task> taskRepository,
			IClientContextScraper clientContextScraper,
			IMapper mapper)
		{
			this.taskRepository = taskRepository;
			this.clientContextScraper = clientContextScraper;
			this.mapper = mapper;
		}

		public List<TaskModel> GetTaskList(TaskModel task)
		{
			bool isAdmin = clientContextScraper.GetClientClaimsRole() == UserRoleEnum.Admin.ToString();

			return taskRepository
				.FetchAll()
				.Where(x => x.UserId == task.UserId || isAdmin == true)
				.Select(x => new TaskModel() 
				{ 
					Id = x.Id, 
					UserId = x.UserId, 
					TaskName = x.TaskName, 
					Status = x.Status 
				})
				.ToList();
		}

		public TaskModel CreateTask(TaskModel task)
		{
			var taskData = mapper.Map<DataModel.Entities.ProjectX.Task>(task);

			taskRepository.Insert(taskData);
			taskRepository.Save();

			task.Id = taskData.Id;

			return task;
		}

		public TaskModel ReadTask(TaskModel task)
		{
			if (CheckIfTaskExists(task))
			{
				return taskRepository
				.FetchAll()
				.Where(x => x.UserId == task.UserId && x.Id == task.Id)
				.Select(x => new TaskModel()
				{
					Id = x.Id,
					UserId = x.UserId,
					TaskName = x.TaskName,
					Status = x.Status
				})
				.FirstOrDefault();
			}
			else
			{
				throw new GenericException(Enums.GenericErrorEnum.TaskDoesNotExist);
			}
		}

		public TaskModel UpdateTask(TaskModel task)
		{
			if (CheckIfTaskExists(task))
			{
				var taskData = mapper.Map<DataModel.Entities.ProjectX.Task>(task);

				taskRepository.Update(taskData);
				taskRepository.Save();

				task.Id = taskData.Id;

				return task;
			}
			else
			{
				throw new GenericException(Enums.GenericErrorEnum.TaskDoesNotExist);
			}
		}

		public void DeleteTask(TaskModel task)
		{
			bool isAdmin = clientContextScraper.GetClientClaimsRole() == UserRoleEnum.Admin.ToString();

			var entity = taskRepository.FetchAll().Where(x => (x.UserId == task.UserId || isAdmin == true) && x.Id == task.Id).FirstOrDefault();

			if (entity != null)
			{		
				taskRepository.Delete(entity);
				taskRepository.Save();
			}
			else
			{
				throw new GenericException(Enums.GenericErrorEnum.TaskDoesNotExist);
			}
		}

		public bool CheckIfTaskExists(TaskModel task)
		{
			return taskRepository.FetchAll().Where(x => x.UserId == task.UserId && x.Id == task.Id).Any();
		}
	}
}
