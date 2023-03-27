using System.Collections.Generic;
using System.Linq;
using Application.Filters;
using Domain.Models;
using Xunit;

namespace Application.Tests.Filters;

public class FindAnyUserFilterTests
{
	[Fact]
	public void FindAnyUserFilter_GetFilter_ReturnsSingleValue()
	{
		var filter = new FindAnyUserFilter
		{
			Name = "User3",
			Email = "user3@mail.com"
		};

		var result = filter.GetFilter(GetUserModels().AsQueryable()).ToList();

		Assert.Single(result);
		Assert.Equal("User3", result[0].Name);
		Assert.Equal("user3@mail.com", result[0].Email);
	}

	[Fact]
	public void FindAnyUserFilter_GetFilter_ReturnsMultipleValues()
	{
		var filter = new FindAnyUserFilter
		{
			Name = "User2",
			Email = "user3@mail.com"
		};

		var result = filter.GetFilter(GetUserModels().AsQueryable()).ToList();

		Assert.Equal(2, result.Count);

		Assert.Equal("User2", result[0].Name);
		Assert.Equal("user2@mail.com", result[0].Email);

		Assert.Equal("User3", result[1].Name);
		Assert.Equal("user3@mail.com", result[1].Email);
	}

	[Fact]
	public void FindAnyUserFilter_GetFilter_ReturnsEmptyList()
	{
		var filter = new FindAnyUserFilter
		{
			Name = "User6",
			Email = "user6@mail.com"
		};

		var result = filter.GetFilter(GetUserModels().AsQueryable()).ToList();

		Assert.Empty(result);
	}

	private static List<UserModel> GetUserModels() => new()
	{
		new() { Name = "User1", Email = "user1@mail.com" },
		new() { Name = "User2", Email = "user2@mail.com" },
		new() { Name = "User3", Email = "user3@mail.com" },
		new() { Name = "User4", Email = "user4@mail.com" },
		new() { Name = "User5", Email = "user5@mail.com" },
	};
}