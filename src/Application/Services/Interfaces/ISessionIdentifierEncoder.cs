namespace Application.Services.Interfaces;

public interface ISessionIdentifierEncoder
{
	(string UserId, DateTime DateTime) ExtractSessionIdentifierVariables(string identifier);

	(string UserId, DateTime DateTime) DecodeSessionIdentifier(string identifier);

	string EncodeSessionIdentifier(string userId, DateTime dateTime);
}