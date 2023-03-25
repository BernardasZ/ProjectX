namespace Application.Services.Options;

public class UserSettings
{
	public const string SelectionName = nameof(UserSettings);

	public int PasswordResetExpirationInMin { get; set; }
}