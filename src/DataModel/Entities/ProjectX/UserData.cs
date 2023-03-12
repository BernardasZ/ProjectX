using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Entities.ProjectX;

[Table("user_data", Schema = "ProjectX")]
public class UserData : BaseEntity
{
	[Column("role_id")]
	public int RoleId { get; set; }

	[Column("user_name")]
	[StringLength(255)]
	public string UserName { get; set; }

	[Column("user_email")]
	[StringLength(255)]
	public string UserEmail { get; set; }

	[Required]
	[Column("pass_hash")]
	[StringLength(255)]
	public string PassHash { get; set; }

	[Column("token_hash")]
	[StringLength(255)]
	public string TokenHash { get; set; }

	[Column("is_token_used", TypeName = "bit(1)")]
	public bool? IsTokenUsed { get; set; }

	[Column("token_expiration_time", TypeName = "datetime")]
	public DateTime? TokenExpirationTime { get; set; }

	[Column("failed_login_count")]
	public int FailedLoginCount { get; set; }

	[ForeignKey(nameof(RoleId))]
	[InverseProperty("UserData")]
	public virtual Role Role { get; set; }
}