using DataModel.Entities.ProjectX;
using DataModel.Enums;
using System.Linq;

namespace DataModel.Filters;

public class RoleEntityFilter : IEntityFilter<Role>
{
	public UserRoleEnum? RoleValue { get; set; }

	public IQueryable<Role> GetFilter(IQueryable<Role> query) => query
		.WhereIf(RoleValue != null, x => x.RoleValue == RoleValue);
}