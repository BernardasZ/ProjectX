using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Entities.ProjectX;

[Table("permission_action", Schema = "ProjectX")]
public class PermissionAction : BaseEntity
{
	public PermissionAction()
	{
		PermissionMapping = new HashSet<PermissionMapping>();
	}

	[Column("controller_id")]
	public int ControllerId { get; set; }

	[Required]
	[Column("action_name")]
	[StringLength(255)]
	public string ActionName { get; set; }

	[ForeignKey(nameof(ControllerId))]
	[InverseProperty(nameof(PermissionController.PermissionAction))]
	public virtual PermissionController Controller { get; set; }

	[InverseProperty("Action")]
	public virtual ICollection<PermissionMapping> PermissionMapping { get; set; }
}