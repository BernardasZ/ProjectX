namespace Domain.Models;

public class PermissionActionModel : ModelBase
{
	public PermissionActionModel()
	{
		PermissionMappings = new HashSet<PermissionMappingModel>();
	}

	public string Name { get; set; }

	public virtual PermissionControllerModel Controller { get; set; }

	public virtual ICollection<PermissionMappingModel> PermissionMappings { get; set; }
}