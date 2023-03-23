using Domain.Extensions;
using Domain.Models;

namespace Domain.Filters;

public class PermissionMappingFilter : IFilter<PermissionMappingModel>
{
	public string RoleName { get; set; }

	public string ControllerName { get; set; }

	public string ActionName { get; set; }

	public bool? AllowAllActions { get; set; }

	public IQueryable<PermissionMappingModel> GetFilter(IQueryable<PermissionMappingModel> query) => query
		.WhereIf(AllowAllActions != null, x => x.AllowAllActions == AllowAllActions)
		.WhereIf(!string.IsNullOrWhiteSpace(ActionName), x => x.Action.Name == ActionName)
		.WhereIf(!string.IsNullOrWhiteSpace(ControllerName), x => x.Controller.Name == ControllerName)
		.WhereIf(!string.IsNullOrWhiteSpace(RoleName), x => x.Role.Name == RoleName);
}