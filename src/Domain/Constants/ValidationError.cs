namespace Domain.Constants;

public static class ValidationError
{
	public const string PasswordRequired = "PasswordIsRequired";
	public const string PasswordLength = "PasswordIsTooShort";
	public const string UserCredentialRequired = "UserNameOrEmailRequired";
	public const string TokenRequired = "TokenIsRequired";
}