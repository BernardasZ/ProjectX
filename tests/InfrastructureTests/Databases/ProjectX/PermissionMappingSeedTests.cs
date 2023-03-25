using System.Linq;
using Infrastructure.Databases.ProjectX.Seeds;
using Xunit;

namespace Infrastructure.Tests.Databases.ProjectX;

public class PermissionMappingSeedTests
{
	[Fact]
	public void GetPermissionMappingsSeed_NotEmpty()
	{
		var mappings = PermissionMappingSeed.GetPermissionMappingSeed()
			.ToList();

		Assert.Equal(3, mappings.Count);
		Assert.True(mappings.All(mapping =>
			mapping.GetType().GetProperties().All(property =>
				property.GetValue(mapping) != null)));
	}
}