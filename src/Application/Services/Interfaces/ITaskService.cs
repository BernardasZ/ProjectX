using Domain.Abstractions;
using Domain.Models;

namespace Application.Services.Interfaces;

public interface ITaskService : IServiceBase<TaskModel>
{
	Task<List<TaskModel>> GetAllTasksByUserIdAsync(int id);
}