using Api.Models.Task;
using System.Collections.Generic;

namespace Api.Services;

public interface ITaskService : IBaseService<TaskModel>
{
	List<TaskModel> GetAllTasksByUserId(int id);
}