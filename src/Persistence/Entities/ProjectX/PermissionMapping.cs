using System.ComponentModel.DataAnnotations;

namespace Persistence.Entities.ProjectX;

public class PermissionMapping : BaseEntity
{
	[Required]
	public bool AllowAllActions { get; set; }

	[Required]
	public virtual PermissionAction Action { get; set; }

	[Required]
	public virtual PermissionController Controller { get; set; }

	[Required]
	public virtual Role Role { get; set; }
}