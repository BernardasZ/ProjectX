using System;
using System.Security.Cryptography;
using System.Text;

namespace ToDoList.Api.Helpers;

public class HashCryptoHelper : IHashCryptoHelper
{
	public string HashString(string text)
	{
		using SHA256 mySHA256 = SHA256.Create();
		byte[] hashValue = mySHA256.ComputeHash(Encoding.Unicode.GetBytes(text));

		return Convert.ToBase64String(hashValue);
	}
}
