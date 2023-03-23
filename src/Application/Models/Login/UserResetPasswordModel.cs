namespace Application.Models.Login;

public class UserResetPasswordModel
{
	public string Token { get; set; }

	public string NewPassHash { get; set; }
}