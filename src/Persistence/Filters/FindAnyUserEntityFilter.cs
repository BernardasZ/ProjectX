using Persistence.Entities.ProjectX;
using System.Linq;

namespace Persistence.Filters;

public class FindAnyUserEntityFilter : IEntityFilter<User>
{
	public string Name { get; set; }

	public string Email { get; set; }

	public IQueryable<User> GetFilter(IQueryable<User> query) => query
		.Where(x => x.Name == Name || x.Email == Email);
}