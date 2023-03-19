using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Persistence.Entities.ProjectX;

public class PermissionController : BaseEntity
{
	public PermissionController()
	{
		PermissionAction = new HashSet<PermissionAction>();
		PermissionMapping = new HashSet<PermissionMapping>();
	}

	[Required]
	[StringLength(255)]
	public string Name { get; set; }

	public virtual ICollection<PermissionAction> PermissionAction { get; set; }

	public virtual ICollection<PermissionMapping> PermissionMapping { get; set; }
}