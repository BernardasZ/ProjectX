namespace ToDoList.Api.Enums;

public enum GenericError
{
	InternalSystemError,
	OperationIsUnavailable,
	TaskDoesNotExist,
	UserDoesNotExist,
	UserExist,
	UserIdentityMissMatch,
	UserResetPasswordTokenIsExpired
}