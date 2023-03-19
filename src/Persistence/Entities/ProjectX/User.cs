using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Persistence.Entities.ProjectX;

public class User : BaseEntity
{
	public User()
	{
		Tasks = new HashSet<Task>();
	}

	[Required]
	[StringLength(255)]
	public string Name { get; set; }

	[Required]
	[StringLength(255)]
	public string Email { get; set; }

	[Required]
	[StringLength(255)]
	public string PassHash { get; set; }

	[StringLength(255)]
	public string TokenHash { get; set; }

	public bool? IsTokenUsed { get; set; }

	public DateTime? TokenExpirationTime { get; set; }

	public int FailedLoginCount { get; set; }

	[Required]
	public virtual Role Role { get; set; }

	public virtual ICollection<Task> Tasks { get; set; }
}