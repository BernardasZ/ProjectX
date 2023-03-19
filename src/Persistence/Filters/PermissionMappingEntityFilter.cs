using Persistence.Entities.ProjectX;
using System.Linq;

namespace Persistence.Filters;

public class PermissionMappingEntityFilter
{
	public string RoleName { get; set; }

	public string ControllerName { get; set; }

	public string ActionName { get; set; }

	public bool? AllowAllActions { get; set; }

	public IQueryable<PermissionMapping> GetFilter(IQueryable<PermissionMapping> query) => query
		.WhereIf(AllowAllActions != null, x => x.AllowAllActions == AllowAllActions)
		.WhereIf(!string.IsNullOrWhiteSpace(ActionName), x => x.Action.Name == ActionName)
		.WhereIf(!string.IsNullOrWhiteSpace(ControllerName), x => x.Controller.Name == ControllerName)
		.WhereIf(!string.IsNullOrWhiteSpace(RoleName), x => x.Role.Name == RoleName);
}