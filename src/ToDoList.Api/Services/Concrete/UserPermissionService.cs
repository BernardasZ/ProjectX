using DataModel.Entities.ProjectX;
using DataModel.Enums;
using DataModel.Repositories;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Api.Helpers;

namespace ToDoList.Api.Services.Concrete
{
	public class UserPermissionService : IUserPermissionService
	{
		private readonly IRepository<UserActionView> userActionViewrepository;
		private readonly ICacheService cacheService;
		private readonly IOptionsMonitor<OptionManager> optionsManager;

		public UserPermissionService(
			IRepository<UserActionView> userActionViewrepository,
			ICacheService cacheService,
			IOptionsMonitor<OptionManager> optionsManager)
		{
			this.userActionViewrepository = userActionViewrepository;
			this.cacheService = cacheService;
			this.optionsManager = optionsManager;
		}

		public bool ValidateUserPermissions(string userRole, string action, string permission)
		{
			return GetPermissions()
				.Where(x => 
							(x.RoleName == userRole || x.RoleName == UserRoleEnum.AllRoles.ToString()) 
							&& x.ActionName == action 
							&& (x.PermissionName == permission || x.PermissionName == ActionPermissionEnum.AllowAll.ToString()))
				.Any();
		}

		public List<UserActionView> GetPermissions()
		{
			string key = optionsManager.CurrentValue.PermissionCacheSettings.Key;
			var expirationTime = TimeSpan.FromMinutes(optionsManager.CurrentValue.PermissionCacheSettings.ExpirationTimeInMin);

			List<UserActionView> permissions = cacheService.GetCache<List<UserActionView>>(key);

			if (permissions == null)
			{
				permissions = userActionViewrepository.FetchAll().ToList();

				cacheService.SetCache<List<UserActionView>>(permissions, key, expirationTime);
			}

			return permissions;
		}
	}
}
