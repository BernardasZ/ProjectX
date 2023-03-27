using System;
using Infrastructure.Helpers;
using Xunit;

namespace Infrastructure.Tests.Helpers;

public class ReflectionHelperTests
{
	[Fact]
	public void GetPropertyValue_ReturnsCorrectValue()
	{
		object testData = new
		{
			Id = 15,
			Name = "TestName",
			Date = new DateTime(2000, 10, 10),
			ItemId = (int?)null
		};

		Assert.Equal(15, ReflectionHelper.GetPropertyValue<int>(testData, "Id"));
		Assert.Equal("TestName", ReflectionHelper.GetPropertyValue<string>(testData, "Name"));
		Assert.Equal(new DateTime(2000, 10, 10), ReflectionHelper.GetPropertyValue<DateTime>(testData, "Date"));
		Assert.Null(ReflectionHelper.GetPropertyValue<int?>(testData, "ItemId"));
	}
}