using Application.Authentication;
using Application.Options;
using Application.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.Services;

public class JwtService : IJwtService
{
	private readonly IOptionsMonitor<Configuration> _configuration;
	private readonly IDateTime _dateTime;

	public JwtService(
		IOptionsMonitor<Configuration> configuration,
		IDateTime dateTime)
	{
		_configuration = configuration;
		_dateTime = dateTime;
	}

	public string ConstructUserJwt(string role, string identifier)
	{
		var claims = new ClaimsIdentity(new Claim[]
		{
			new Claim(ClaimTypes.Role, role),
			new Claim(ClaimTypes.NameIdentifier, identifier)
		});

		var key = Encoding.ASCII.GetBytes(_configuration.CurrentValue.Jwt.JWTSecret);
		var days = _configuration.CurrentValue.Jwt.JWTExpirationInDay;

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