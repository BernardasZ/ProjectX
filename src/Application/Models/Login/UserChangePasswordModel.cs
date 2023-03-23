namespace Application.Models.Login;

public class UserChangePasswordModel
{
	public string Email { get; set; }

	public string NewPassHash { get; set; }

	public string OldPassHash { get; set; }
}