using DataModel.Entities.ProjectX;
using System.Collections.Generic;

namespace ToDoList.Api.Services;

public interface IPermissionCacheService
{
	List<PermissionView> GetCache();
}