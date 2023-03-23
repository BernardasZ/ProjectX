using Domain.Enums;

namespace Domain.Models;

public class RoleModel : ModelBase
{
	public RoleModel()
	{
		Users = new HashSet<UserModel>();
		PermissionMappings = new HashSet<PermissionMappingModel>();
	}

	public string Name { get; set; }

	public UserRole Value { get; set; }

	public virtual ICollection<UserModel> Users { get; set; }

	public virtual ICollection<PermissionMappingModel> PermissionMappings { get; set; }
}