using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Api.Models;

namespace ToDoList.Api.Services
{
	public interface ITaskService
	{
		List<TaskModel> GetTaskList(TaskModel task);
		TaskModel CreateTask(TaskModel task);
		TaskModel ReadTask(TaskModel task);
		TaskModel UpdateTask(TaskModel task);
		void DeleteTask(TaskModel task);
		bool CheckIfTaskExists(TaskModel task);
	}
}
