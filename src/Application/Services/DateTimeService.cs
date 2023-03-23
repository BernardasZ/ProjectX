using Application.Services.Interfaces;

namespace Application.Services;

public class DateTimeService : IDateTime
{
	public DateTime GetDateTime() => DateTime.UtcNow;
}