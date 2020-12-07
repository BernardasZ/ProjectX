using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.Api.Authorization
{
	internal class ActionPermissionRequirement : IAuthorizationRequirement
	{
		public ActionPermissionRequirement()
		{
		}
	}
}
