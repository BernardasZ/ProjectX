using DataModel.Entities.ProjectX;
using System.Linq;

namespace DataModel.Filters;

public class FindAnyUserEntityFilter : IEntityFilter<UserData>
{
	public string UserName { get; set; }

	public string UserEmail { get; set; }

	public IQueryable<UserData> GetFilter(IQueryable<UserData> query) => query
		.Where(x => x.UserName == UserName || x.UserEmail == UserEmail);
}