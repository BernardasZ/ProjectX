using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.Api.Enums
{
	public enum GenericErrorEnum
	{
		InternalSystemError = 0,

		UserDoesNotExist = 100,
		TaskDoesNotExist = 101
	}
}
