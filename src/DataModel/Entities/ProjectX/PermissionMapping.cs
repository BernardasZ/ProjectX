using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Entities.ProjectX;

[Table("permission_mapping", Schema = "ProjectX")]
public class PermissionMapping : BaseEntity
{
	[Column("role_id")]
	public int RoleId { get; set; }

	[Column("controller_id")]
	public int ControllerId { get; set; }

	[Column("action_id")]
	public int ActionId { get; set; }

	[Column("allow_all_actions", TypeName = "bit(1)")]
	public bool AllowAllActions { get; set; }

	[ForeignKey(nameof(ActionId))]
	[InverseProperty(nameof(PermissionAction.PermissionMapping))]
	public virtual PermissionAction Action { get; set; }

	[ForeignKey(nameof(ControllerId))]
	[InverseProperty(nameof(PermissionController.PermissionMapping))]
	public virtual PermissionController Controller { get; set; }

	[ForeignKey(nameof(RoleId))]
	[InverseProperty("PermissionMapping")]
	public virtual Role Role { get; set; }
}