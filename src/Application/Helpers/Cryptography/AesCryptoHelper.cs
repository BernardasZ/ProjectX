using Application.Options;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace Application.Helpers.Cryptography;

public class AesCryptoHelper : IAesCryptoHelper
{
	private readonly IOptionsMonitor<CryptographySettings> _appSettings;

	public AesCryptoHelper(IOptionsMonitor<CryptographySettings> appSettings)
	{
		_appSettings = appSettings;
	}

	public string EncryptString(string text)
	{
		var clearBytes = Encoding.Unicode.GetBytes(text);

		using (var encryptor = CreateAesEncryptor())
		{
			var stream = GetEncryptorStream(clearBytes, encryptor.CreateEncryptor());

			return Convert.ToBase64String(stream.ToArray());
		}
	}

	public string DecryptString(string cipherText)
	{
		var cipherBytes = Convert.FromBase64String(cipherText.Replace(" ", "+"));

		using (var encryptor = CreateAesEncryptor())
		{
			var stream = GetEncryptorStream(cipherBytes, encryptor.CreateDecryptor());

			return Encoding.Unicode.GetString(stream.ToArray());
		}
	}

	private static MemoryStream GetEncryptorStream(byte[] bytesToWrite, ICryptoTransform encryptor)
	{
		var stream = new MemoryStream();

		using (var cs = new CryptoStream(stream, encryptor, CryptoStreamMode.Write))
		{
			cs.Write(bytesToWrite, 0, bytesToWrite.Length);
			cs.Close();
		}

		return stream;
	}

	private Aes CreateAesEncryptor()
	{
		var encryptor = Aes.Create();
		var pdb = new Rfc2898DeriveBytes(
			_appSettings.CurrentValue.AesKey,
			Encoding.Unicode.GetBytes(_appSettings.CurrentValue.AlgorithmIV));

		encryptor.Key = pdb.GetBytes(32);
		encryptor.IV = pdb.GetBytes(16);

		return encryptor;
	}
}