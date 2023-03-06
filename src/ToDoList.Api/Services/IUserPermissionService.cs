using DataModel.Entities.ProjectX;
using System.Collections.Generic;

namespace ToDoList.Api.Services;

public interface IUserPermissionService
{
	bool ValidateUserPermissions();

	List<PermissionView> GetPermissions();
}
