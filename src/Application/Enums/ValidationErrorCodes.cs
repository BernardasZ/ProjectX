namespace Application.Enums;

public enum ValidationErrorCodes
{
	TaskDoesNotExist,
	UserExist,
	UserDoesNotExist,
	UserIdentityMissMatch,
	UserResetPasswordTokenIsExpired,
	UserPasswordIsIncorrect,
	UserIsBlocked
}