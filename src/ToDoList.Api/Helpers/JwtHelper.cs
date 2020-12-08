using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ToDoList.Api.Helpers
{
	public interface IJwtHelper
	{
		string ConstructUserJwt(ClaimsIdentity subject);
	}

	public class JwtHelper : IJwtHelper
	{
		private readonly IClientContextScraper clientContextScraper;
		private readonly IOptionsMonitor<OptionManager> optionsManager;

		public JwtHelper(
			IClientContextScraper clientContextScraper,
			IOptionsMonitor<OptionManager> optionsManager)
		{
			this.clientContextScraper = clientContextScraper;
			this.optionsManager = optionsManager;
		}

		public string ConstructUserJwt(ClaimsIdentity subject)
		{
			var key = Encoding.ASCII.GetBytes(optionsManager.CurrentValue.Jwt.JWTSecret);
			var days = optionsManager.CurrentValue.Jwt.JWTExpirationInDay;

			var tokenHandler = new JwtSecurityTokenHandler();

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = subject,
				Expires = DateTime.UtcNow.AddDays(days),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};

			return tokenHandler.CreateEncodedJwt(tokenDescriptor);
		}
	}
}
