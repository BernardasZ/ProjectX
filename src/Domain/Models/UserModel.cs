namespace Domain.Models;

public class UserModel : ModelBase
{
	public UserModel()
	{
		Tasks = new HashSet<TaskModel>();
		UserSessions = new HashSet<UserSessionModel>();
	}

	public string Name { get; set; }

	public string Email { get; set; }

	public string PassHash { get; set; }

	public string TokenHash { get; set; }

	public bool? IsTokenUsed { get; set; }

	public DateTime? TokenExpirationTime { get; set; }

	public int FailedLoginCount { get; set; }

	public virtual RoleModel Role { get; set; }

	public virtual ICollection<TaskModel> Tasks { get; set; }

	public virtual ICollection<UserSessionModel> UserSessions { get; set; }
}