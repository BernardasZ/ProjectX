using DataModel.Entities.ProjectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.Api.Services
{
	public interface IUserPermissionService
	{
		bool ValidateUserPermissions(string userRole, string action, string permission);
		List<UserActionView> GetPermissions();
	}
}
