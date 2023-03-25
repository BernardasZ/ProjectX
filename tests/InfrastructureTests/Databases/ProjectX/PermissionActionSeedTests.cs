using System.Linq;
using System.Reflection;
using Infrastructure.Databases.ProjectX.Seeds;
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
			.Select(controller => new
			{
				Id = (int?)controller?.GetType()?.GetProperty("Id")?.GetValue(controller),
				Name = (string?)controller?.GetType()?.GetProperty("Name")?.GetValue(controller)
			})
			.ToList();

		var permissionActionNames = PermissionActionSeed.GetPermissionActionSeed()
			.Select(action => new
			{
				Name = (string?)(action?.GetType()?.GetProperty("Name")?.GetValue(action)),
				ControllerId = (int?)action?.GetType()?.GetProperty("ControllerId")?.GetValue(action)
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