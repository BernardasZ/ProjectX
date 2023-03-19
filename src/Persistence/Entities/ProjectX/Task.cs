using Persistence.Enums;
using System.ComponentModel.DataAnnotations;

namespace Persistence.Entities.ProjectX;

public class Task : BaseEntity
{
	[Required]
	[StringLength(1000)]
	public string Name { get; set; }

	[Required]
	public TaskStatus Status { get; set; }

	[Required]
	public virtual User User { get; set; }
}