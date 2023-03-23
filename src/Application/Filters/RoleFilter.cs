using Application.Extensions;
using Domain.Enums;
using Domain.Filters;
using Domain.Models;

namespace Application.Filters;

public class RoleFilter : IFilter<RoleModel>
{
	public UserRole? Value { get; set; }

	public IQueryable<RoleModel> GetFilter(IQueryable<RoleModel> query) => query
		.WhereIf(Value != null, x => x.Value == Value);
}