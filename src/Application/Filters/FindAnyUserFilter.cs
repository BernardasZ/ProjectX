using Domain.Filters;
using Domain.Models;

namespace Application.Filters;

public class FindAnyUserFilter : IFilter<UserModel>
{
	public int? Id { get; set; }

	public string Name { get; set; }

	public string Email { get; set; }

	public IQueryable<UserModel> GetFilter(IQueryable<UserModel> query) => query
		.Where(x => (x.Name == Name || x.Email == Email)
				&& (Id == null || x.Id != Id));
}