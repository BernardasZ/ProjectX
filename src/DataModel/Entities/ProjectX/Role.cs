using DataModel.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Entities.ProjectX;

[Table("role", Schema = "ProjectX")]
public class Role : BaseEntity
{
	public Role()
	{
		PermissionMapping = new HashSet<PermissionMapping>();
		UserData = new HashSet<UserData>();
	}

	[Required]
	[Column("role_name")]
	[StringLength(50)]
	public string RoleName { get; set; }

	[Column("role_value", TypeName = "tinyint unsigned")]
	public UserRoleEnum RoleValue { get; set; }

	[InverseProperty("Role")]
	public virtual ICollection<PermissionMapping> PermissionMapping { get; set; }

	[InverseProperty("Role")]
	public virtual ICollection<UserData> UserData { get; set; }
}