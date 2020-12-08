using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.Api.Constants
{
	public static class Permissions
	{
		public const string CheckPermissions = "CheckPermissions";
	}

	public static class ValidationError
	{
		public const string PasswordRequired = "PasswordIsRequired";
		public const string PasswordLength = "PasswordIsTooShort";
		public const string UserCredentialRequired = "UserNameOrEmailRequired";
		public const string TokenRequired = "TokenIsRequired";
	}
}
