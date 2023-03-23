using Application.Extensions;
using Domain.Filters;
using Domain.Models;
using TaskStatus = Domain.Enums.TaskStatus;

namespace Application.Filters;

public class TaskFilter : IFilter<TaskModel>
{
	public int? Id { get; set; }

	public int? UserId { get; set; }

	public string Name { get; set; }

	public TaskStatus? Status { get; set; }

	public IQueryable<TaskModel> GetFilter(IQueryable<TaskModel> query) => query
		.WhereIf(Id != null, x => x.Id == Id)
		.WhereIf(UserId != null, x => x.User.Id == UserId)
		.WhereIf(Status != null, x => x.Status == Status)
		.WhereIf(!string.IsNullOrWhiteSpace(Name), x => x.Name == Name);
}