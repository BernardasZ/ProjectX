namespace Domain.Enums;

public enum GenericError
{
	InternalSystemError,
	OperationIsUnavailable,
	TaskDoesNotExist,
	UserExist,
	UserDoesNotExist,
	UserIdentityMissMatch,
	UserResetPasswordTokenIsExpired,
	UserPasswordIsIncorrect,
	UserIsBlocked
}