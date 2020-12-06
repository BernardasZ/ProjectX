using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.Api.Services
{
	public interface IUserService
	{
		bool IsValidUserSession();
	}
}
