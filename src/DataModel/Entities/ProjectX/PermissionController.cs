using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Entities.ProjectX;

[Table("permission_controller", Schema = "ProjectX")]
public class PermissionController : BaseEntity
{
	public PermissionController()
	{
		PermissionAction = new HashSet<PermissionAction>();
		PermissionMapping = new HashSet<PermissionMapping>();
	}

	[Required]
	[Column("controller_name")]
	[StringLength(255)]
	public string ControllerName { get; set; }

	[InverseProperty("Controller")]
	public virtual ICollection<PermissionAction> PermissionAction { get; set; }

	[InverseProperty("Controller")]
	public virtual ICollection<PermissionMapping> PermissionMapping { get; set; }
}