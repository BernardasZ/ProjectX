using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Persistence.Entities.ProjectX;

public class PermissionAction : BaseEntity
{
	public PermissionAction()
	{
		PermissionMapping = new HashSet<PermissionMapping>();
	}

	[Required]
	[StringLength(255)]
	public string Name { get; set; }

	[Required]
	public virtual PermissionController Controller { get; set; }

	public virtual ICollection<PermissionMapping> PermissionMapping { get; set; }
}