using Domain.Enums;
using Domain.Extensions;
using Domain.Models;

namespace Domain.Filters;

public class RoleFilter : IFilter<RoleModel>
{
	public UserRole? Value { get; set; }

	public IQueryable<RoleModel> GetFilter(IQueryable<RoleModel> query) => query
		.WhereIf(Value != null, x => x.Value == Value);
}