using Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Api.Services;

public class ClientContextScraperService : IClientContextScraper
{
	private readonly IHttpContextAccessor _httpContextAccessor;

	public ClientContextScraperService(IHttpContextAccessor httpContextAccessor)
	{
		_httpContextAccessor = httpContextAccessor;
	}

	public string GetClientIpAddress() => _httpContextAccessor.HttpContext.Request.Headers.ContainsKey("X-Forwarded-For")
			? _httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"].ToString().Split(":")[0].Split(',')[0].Trim()
			: _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();

	public string GetClientClaimsIdentityName() => GetUserClaims(ClaimTypes.NameIdentifier);

	public string GetClientClaimsRole() => GetUserClaims(ClaimTypes.Role);

	public string GetControllerName() => GetRouteValues("controller");

	public string GetActionrName() => GetRouteValues("action");

	private string GetUserClaims(string claimType) => !_httpContextAccessor.HttpContext.User.Claims.Any(c => c.Type == claimType)
			? string.Empty
			: _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == claimType).Value;

	public string GetRouteValues(string value) =>
		_httpContextAccessor.HttpContext.Request.RouteValues.GetValueOrDefault(value).ToString();
}