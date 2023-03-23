using Domain.Models;

namespace Application.Services.Interfaces;

public interface ITaskService : IServiceBase<TaskModel>
{
	List<TaskModel> GetAllTasksByUserId(int id);
}