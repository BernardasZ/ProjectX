namespace Domain.Models;

public class PermissionControllerModel : ModelBase
{
	public PermissionControllerModel()
	{
		PermissionActions = new HashSet<PermissionActionModel>();
		PermissionMappings = new HashSet<PermissionMappingModel>();
	}

	public string Name { get; set; }

	public virtual ICollection<PermissionActionModel> PermissionActions { get; set; }

	public virtual ICollection<PermissionMappingModel> PermissionMappings { get; set; }
}