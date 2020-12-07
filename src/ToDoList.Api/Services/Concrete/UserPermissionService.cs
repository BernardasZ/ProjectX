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
		private readonly ICacheService<List<PermissionView>> cacheService;
		private readonly IClientContextScraper clientContextScraper;

		public UserPermissionService(
			ICacheService<List<PermissionView>> cacheService,
			IClientContextScraper clientContextScraper)
		{
			this.cacheService = cacheService;
			this.clientContextScraper = clientContextScraper;
		}

		public bool ValidateUserPermissions()
		{
			string userRole = clientContextScraper.GetClientClaimsRole();
			string controller = clientContextScraper.GetControllerName();
			string action = clientContextScraper.GetActionrName();

			if (string.IsNullOrWhiteSpace(userRole) || string.IsNullOrWhiteSpace(controller) || string.IsNullOrWhiteSpace(action))
			{
				return false;
			}

			return GetPermissions()
				.Where(x => 
							(x.RoleName == userRole || x.RoleName == UserRoleEnum.AllRoles.ToString()) 
							&& x.ControllerName == controller 
							&& (x.ActionName == action || x.AllowAllActions))
				.Any();
		}

		public List<PermissionView> GetPermissions()
		{
			return cacheService.GetCache();
		}
	}
}
