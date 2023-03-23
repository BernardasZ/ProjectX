using Domain.Models;

namespace Domain.Filters;

public class FindAnyUserFilter : IFilter<UserModel>
{
	public string Name { get; set; }

	public string Email { get; set; }

	public IQueryable<UserModel> GetFilter(IQueryable<UserModel> query) => query
		.Where(x => x.Name == Name || x.Email == Email);
}