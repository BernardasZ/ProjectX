﻿namespace ToDoList.Api.Enums
{
	public enum GenericErrorEnum
	{
		InternalSystemError = 0,
		OperationIsUnavailable = 1,
		TaskDoesNotExist = 100,
		UserDoesNotExist = 200,	
		UserExist = 201,
		UserIdentityMissMatch = 202,
		UserResetPasswordTokenIsExpired = 203
	}
}
