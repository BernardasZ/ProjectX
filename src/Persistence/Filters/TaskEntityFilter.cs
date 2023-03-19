using Persistence.Entities.ProjectX;
using Persistence.Enums;
using System.Linq;

namespace Persistence.Filters;

public class TaskEntityFilter : IEntityFilter<Task>
{
	public int? Id { get; set; }

	public int? UserId { get; set; }

	public string Name { get; set; }

	public TaskStatus? Status { get; set; }

	public IQueryable<Task> GetFilter(IQueryable<Task> query) => query
		.WhereIf(Id != null, x => x.Id == Id)
		.WhereIf(UserId != null, x => x.User.Id == UserId)
		.WhereIf(Status != null, x => x.Status == Status)
		.WhereIf(!string.IsNullOrWhiteSpace(Name), x => x.Name == Name);
}