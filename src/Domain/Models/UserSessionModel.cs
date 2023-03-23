namespace Domain.Models;

public class UserSessionModel : ModelBase
{
	public string SessionIdentifier { get; set; }

	public DateTime CreateDt { get; set; }

	public string Ip { get; set; }

	public virtual UserModel User { get; set; }
}