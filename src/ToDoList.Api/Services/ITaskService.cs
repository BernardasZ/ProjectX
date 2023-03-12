using System.Collections.Generic;
using ToDoList.Api.Models.Task;

namespace ToDoList.Api.Services;

public interface ITaskService : IBaseService<TaskModel>
{
	List<TaskModel> GetAllTasksByUserId(int id);
}