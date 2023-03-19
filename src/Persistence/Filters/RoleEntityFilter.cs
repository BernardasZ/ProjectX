using Persistence.Entities.ProjectX;
using Persistence.Enums;
using System.Linq;

namespace Persistence.Filters;

public class RoleEntityFilter : IEntityFilter<Role>
{
	public UserRole? Value { get; set; }

	public IQueryable<Role> GetFilter(IQueryable<Role> query) => query
		.WhereIf(Value != null, x => x.Value == Value);
}