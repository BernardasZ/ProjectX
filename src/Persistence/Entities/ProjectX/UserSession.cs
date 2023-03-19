using System;
using System.ComponentModel.DataAnnotations;

namespace Persistence.Entities.ProjectX;

public class UserSession : BaseEntity
{
	[Required]
	[StringLength(255)]
	public string SessionIdentifier { get; set; }

	[Required]
	public DateTime CreateDt { get; set; }

	[Required]
	[StringLength(15)]
	public string Ip { get; set; }
}