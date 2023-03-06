using System.Collections.Generic;
using ToDoList.Api.Models.Task;

namespace ToDoList.Api.Services;

public interface ITaskService
{
	List<TaskModel> GetTaskList(TaskModel model);

	TaskModel CreateTask(TaskModel model);

	TaskModel ReadTask(TaskModel model);

	TaskModel UpdateTask(TaskModel model);

	void DeleteTask(TaskModel model);
}
