using System.ComponentModel.DataAnnotations;

namespace Persistence.Entities.ProjectX;

public class BaseEntity
{
	[Required]
	public int? Id { get; set; }
}