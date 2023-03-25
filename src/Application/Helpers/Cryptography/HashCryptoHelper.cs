using System.Security.Cryptography;
using System.Text;

namespace Application.Helpers.Cryptography;

public class HashCryptoHelper : IHashCryptoHelper
{
	public string GetHashString(string text)
	{
		using var mySHA256 = SHA256.Create();
		var hashValue = mySHA256.ComputeHash(Encoding.Unicode.GetBytes(text));

		return Convert.ToBase64String(hashValue);
	}
}