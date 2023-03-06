using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace ToDoList.Api.Helpers;

public interface IClientContextScraper
{
	string GetClientClaimsIdentityName();

	string GetClientClaimsRole();

	string GetClientIpAddress();

	string GetControllerName();

	string GetActionrName();
}

public class ClientContextScraper : IClientContextScraper
{
	private readonly IHttpContextAccessor _httpContextAccessor;

	public ClientContextScraper(IHttpContextAccessor httpContextAccessor)
	{
		_httpContextAccessor = httpContextAccessor;
	}

	public string GetClientIpAddress()
	{
		var remoteIpAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();

		if (_httpContextAccessor.HttpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
		{
			remoteIpAddress = _httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"].ToString().Split(":")[0].Split(',')[0].Trim();
		}

		return remoteIpAddress;
	}

	public string GetClientClaimsIdentityName() => !_httpContextAccessor.HttpContext.User.Claims.Any(c => c.Type == ClaimTypes.Name)
			? string.Empty
			: _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;

	public string GetClientClaimsRole() => !_httpContextAccessor.HttpContext.User.Claims.Any(c => c.Type == ClaimTypes.Role)
			? string.Empty
			: _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;

	public string GetControllerName() =>
		_httpContextAccessor.HttpContext.Request.RouteValues.GetValueOrDefault("controller").ToString();

	public string GetActionrName() =>
		_httpContextAccessor.HttpContext.Request.RouteValues.GetValueOrDefault("action").ToString();
}
