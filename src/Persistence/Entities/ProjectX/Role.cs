using Persistence.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Persistence.Entities.ProjectX;

public class Role : BaseEntity
{
	public Role()
	{
		PermissionMapping = new HashSet<PermissionMapping>();
		UserData = new HashSet<User>();
	}

	[Required]
	[StringLength(50)]
	public string Name { get; set; }

	[Required]
	public UserRole Value { get; set; }

	public virtual ICollection<PermissionMapping> PermissionMapping { get; set; }

	public virtual ICollection<User> UserData { get; set; }
}