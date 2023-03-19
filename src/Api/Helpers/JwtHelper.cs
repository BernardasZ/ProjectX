using Api.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.Helpers;

public class JwtHelper : IJwtHelper
{
	private readonly IOptionsMonitor<OptionManager> _optionsManager;
	private readonly IAesCryptoHelper _aesCryptoHelper;

	public JwtHelper(
		IOptionsMonitor<OptionManager> optionsManager,
		IAesCryptoHelper aesCryptoHelper)
	{
		_optionsManager = optionsManager;
		_aesCryptoHelper = aesCryptoHelper;
	}

	public string ConstructUserJwt(string role, string userId)
	{
		var claims = new ClaimsIdentity(new Claim[]
		{
			new Claim(ClaimTypes.Name, _aesCryptoHelper.EncryptString(userId)),
			new Claim(ClaimTypes.Role, role)
		});

		var key = Encoding.ASCII.GetBytes(_optionsManager.CurrentValue.Jwt.JWTSecret);
		var days = _optionsManager.CurrentValue.Jwt.JWTExpirationInDay;

		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = claims,
			Expires = DateTime.UtcNow.AddDays(days),
			SigningCredentials = new SigningCredentials(
				new SymmetricSecurityKey(key),
				SecurityAlgorithms.HmacSha256Signature)
		};

		return new JwtSecurityTokenHandler().CreateEncodedJwt(tokenDescriptor);
	}
}