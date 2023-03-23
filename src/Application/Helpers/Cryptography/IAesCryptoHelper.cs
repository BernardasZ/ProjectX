namespace Application.Helpers.Cryptography;

public interface IAesCryptoHelper
{
	string EncryptString(string text);

	string DecryptString(string text);
}