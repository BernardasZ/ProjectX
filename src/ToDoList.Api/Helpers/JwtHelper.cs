using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ToDoList.Api.Helpers;

public interface IJwtHelper
{
	string ConstructUserJwt(ClaimsIdentity subject);
}

public class JwtHelper : IJwtHelper
{
	private readonly IOptionsMonitor<OptionManager> _optionsManager;

	public JwtHelper(
		IOptionsMonitor<OptionManager> optionsManager)
	{
		_optionsManager = optionsManager;
	}

	public string ConstructUserJwt(ClaimsIdentity subject)
	{
		var key = Encoding.ASCII.GetBytes(_optionsManager.CurrentValue.Jwt.JWTSecret);
		var days = _optionsManager.CurrentValue.Jwt.JWTExpirationInDay;

		var tokenHandler = new JwtSecurityTokenHandler();

		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = subject,
			Expires = DateTime.UtcNow.AddDays(days),
			SigningCredentials = new SigningCredentials(
				new SymmetricSecurityKey(key),
				SecurityAlgorithms.HmacSha256Signature)
		};

		return tokenHandler.CreateEncodedJwt(tokenDescriptor);
	}
}
