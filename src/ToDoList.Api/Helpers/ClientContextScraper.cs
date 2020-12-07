using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ToDoList.Api.Helpers
{
	public interface IClientContextScraper
	{
		string GetClientClaimsName();
		string GetClientClaimsRole();
		string GetClientIpAddress();
		string GetControllerName();
		string GetActionrName();
	}

	public class ClientContextScraper : IClientContextScraper
	{
		private readonly IHttpContextAccessor httpContextAccessor;
		public ClientContextScraper(IHttpContextAccessor httpContextAccessor)
		{
			this.httpContextAccessor = httpContextAccessor;
		}

		public string GetClientIpAddress()
		{
			string remoteIpAddress = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();

			if (httpContextAccessor.HttpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
			{
				remoteIpAddress = httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"].ToString().Split(":")[0].Split(',')[0].Trim();
			}

			return remoteIpAddress;
		}

		public string GetClientClaimsName()
		{
			return httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
		}

		public string GetClientClaimsRole()
		{
			return httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
		}

		public string GetControllerName()
		{
			return httpContextAccessor.HttpContext.Request.RouteValues.GetValueOrDefault("controller").ToString();
		}

		public string GetActionrName()
		{
			return httpContextAccessor.HttpContext.Request.RouteValues.GetValueOrDefault("action").ToString();
		}
	}
}
