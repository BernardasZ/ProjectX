using Domain.Abstractions;

namespace Application.Services;

public class DateTimeService : IDateTime
{
	public DateTime GetDateTime() => DateTime.UtcNow;
}