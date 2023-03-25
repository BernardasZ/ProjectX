using Api.Options;
using Application.Authentication;
using Domain.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.Services;

public class JwtService : IJwtService
{
	private readonly IOptionsMonitor<JwtSettings> _jwtSettings;
	private readonly IDateTime _dateTime;

	public JwtService(
		IOptionsMonitor<JwtSettings> jwtSettings,
		IDateTime dateTime)
	{
		_jwtSettings = jwtSettings;
		_dateTime = dateTime;
	}

	public string ConstructUserJwt(string role, string identifier)
	{
		var claims = new ClaimsIdentity(new Claim[]
		{
			new Claim(ClaimTypes.Role, role),
			new Claim(ClaimTypes.NameIdentifier, identifier)
		});

		var key = Encoding.ASCII.GetBytes(_jwtSettings.CurrentValue.JWTSecret);
		var days = _jwtSettings.CurrentValue.JWTExpirationInDay;

		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = claims,
			Expires = _dateTime.GetDateTime().AddDays(days),
			SigningCredentials = new SigningCredentials(
				new SymmetricSecurityKey(key),
				SecurityAlgorithms.HmacSha256Signature)
		};

		return new JwtSecurityTokenHandler().CreateEncodedJwt(tokenDescriptor);
	}
}