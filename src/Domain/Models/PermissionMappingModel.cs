namespace Domain.Models;

public class PermissionMappingModel : ModelBase
{
	public bool AllowAllActions { get; set; }

	public virtual PermissionActionModel Action { get; set; }

	public virtual PermissionControllerModel Controller { get; set; }

	public virtual RoleModel Role { get; set; }
}