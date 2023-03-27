using System.Linq;
using System.Reflection;
using Infrastructure.Databases.ProjectX.Seeds;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Xunit;

namespace Infrastructure.Tests.Databases.ProjectX;

public class PermissionActionSeedTests
{
	[Fact]
	public void PermissionControllersSeed_AllPermissionActionsAreSetToSeed()
	{
		var controllerMethodNamesWithPermissionCheckAttributes = Assembly.LoadFrom("Api")
			.DefinedTypes
			.Where(type => type.BaseType == typeof(ControllerBase))
			.SelectMany(controller =>
				controller.GetMethods().Where(method =>
					method.GetCustomAttribute(typeof(HttpMethodAttribute)) != null))
			.Where(method =>
				method.GetCustomAttribute(typeof(AuthorizeAttribute)) != null
				|| method.ReflectedType?.GetCustomAttribute(typeof(AuthorizeAttribute)) != null)
			.Select(method =>
				$"{method.ReflectedType?.Name.Replace("Controller", string.Empty)}{method.Name}")
			.ToList();

		var permissionControllers = PermissionControllerSeed.GetPermissionControllerSeed()
			.ToList();

		var permissionActionNames = PermissionActionSeed.GetPermissionActionSeed()
			.Select(action => new
			{
				Name = ReflectionHelper.GetPropertyValue<string>(action, "Name"),
				ControllerId = ReflectionHelper.GetPropertyValue<int?>(action, "ControllerId")
			})
			.Where(action => action.Name != "All")
			.Select(action =>
				$"{permissionControllers.FirstOrDefault(controller => controller.Id == action.ControllerId)?.Name}{action.Name}")
			.ToList();

		Assert.Equal(controllerMethodNamesWithPermissionCheckAttributes.Count, permissionActionNames.Count);
		Assert.True(controllerMethodNamesWithPermissionCheckAttributes.All(name =>
			permissionActionNames.Contains(name)));
	}
}