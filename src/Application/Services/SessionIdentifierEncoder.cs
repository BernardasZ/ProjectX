using Application.Helpers.Cryptography;
using Application.Services.Interfaces;

namespace Application.Services;

public class SessionIdentifierEncoder : ISessionIdentifierEncoder
{
	private const string _splitChar = "-";
	private readonly IAesCryptoHelper _aesCryptoHelper;

	public SessionIdentifierEncoder(IAesCryptoHelper aesCryptoHelper)
	{
		_aesCryptoHelper = aesCryptoHelper;
	}

	public (string UserId, DateTime DateTime) ExtractSessionIdentifierVariables(string identifier) =>
		(identifier[..identifier.IndexOf(_splitChar)],
		new DateTime(long.Parse(
			identifier[(identifier.IndexOf(_splitChar) + 1)..])));

	public (string UserId, DateTime DateTime) DecodeSessionIdentifier(string identifier) =>
		DecodeSessionIdentifier(_aesCryptoHelper.DecryptString(identifier));

	public string EncodeSessionIdentifier(string userId, DateTime dateTime) =>
		_aesCryptoHelper.EncryptString(
			$"{userId}{_splitChar}{dateTime.Ticks}");
}