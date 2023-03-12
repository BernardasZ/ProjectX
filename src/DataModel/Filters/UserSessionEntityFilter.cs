using DataModel.Entities.ProjectX;
using System;
using System.Linq;

namespace DataModel.Filters;

public class UserSessionEntityFilter : IEntityFilter<UserSession>
{
	public string SessionIdentifier { get; set; }

	public string Ip { get; set; }

	public DateTime? CreateDt { get; set; }

	public IQueryable<UserSession> GetFilter(IQueryable<UserSession> query) => query
		.WhereIf(string.IsNullOrWhiteSpace(SessionIdentifier), x => x.SessionIdentifier == SessionIdentifier)
		.WhereIf(string.IsNullOrWhiteSpace(Ip), x => x.Ip == Ip)
		.WhereIf(CreateDt != null, x => x.CreateDt == CreateDt);
}