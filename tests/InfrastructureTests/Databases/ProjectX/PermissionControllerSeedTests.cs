using System.Linq;
using System.Reflection;
using Infrastructure.Databases.ProjectX.Seeds;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Infrastructure.Tests.Databases.ProjectX;

public class PermissionControllerSeedTests
{
	[Fact]
	public void PermissionControllersSeed_AllPermissionControllersAreSetToSeed()
	{
		var permissionControllers = PermissionControllerSeed.GetPermissionControllerSeed()
			.Select(x => x.Name)
			.ToList();

		var controllers = Assembly.LoadFrom("Api")
			.DefinedTypes
			.Where(type => type.BaseType == typeof(ControllerBase))
			.Where(controller =>
				controller.GetMethods().Any(method => method.GetCustomAttribute(typeof(AuthorizeAttribute)) != null
					&& controller.GetCustomAttribute(typeof(AuthorizeAttribute)) == null)
				|| controller.GetCustomAttribute(typeof(AuthorizeAttribute)) != null)
			.ToList();

		Assert.Equal(controllers.Count, permissionControllers.Count);
		Assert.True(controllers.All(controller =>
			permissionControllers.Contains(
				controller.Name.Replace("Controller", string.Empty))));
	}
}