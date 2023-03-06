using System;
using System.Security.Cryptography;
using System.Text;

namespace ToDoList.Api.Helpers;

public interface IHashCryptoHelper
{
	string HashString(string text);
}

public class HashCryptoHelper : IHashCryptoHelper
{
	public string HashString(string text)
	{
		byte[] clearBytes = Encoding.Unicode.GetBytes(text);

		using (SHA256 mySHA256 = SHA256.Create())
		{
			byte[] hashValue = mySHA256.ComputeHash(clearBytes);
			text = Convert.ToBase64String(hashValue);
		}

		return text;
	}
}
