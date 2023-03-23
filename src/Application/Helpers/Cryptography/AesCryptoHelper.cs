using Application.Options;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace Application.Helpers.Cryptography;

public class AesCryptoHelper : IAesCryptoHelper
{
	private readonly IOptionsMonitor<Configuration> _configuration;

	public AesCryptoHelper(IOptionsMonitor<Configuration> configuration)
	{
		_configuration = configuration;
	}

	public string EncryptString(string text)
	{
		byte[] clearBytes = Encoding.Unicode.GetBytes(text);

		using (var encryptor = CreateAesEncryptor())
		{
			var stream = GetEncryptorStream(clearBytes, encryptor.CreateEncryptor());

			return Convert.ToBase64String(stream.ToArray());
		}
	}

	public string DecryptString(string cipherText)
	{
		byte[] cipherBytes = Convert.FromBase64String(cipherText.Replace(" ", "+"));

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
			_configuration.CurrentValue.AppSettings.AesKey,
			Encoding.Unicode.GetBytes(_configuration.CurrentValue.AppSettings.AlgorithmIV));

		encryptor.Key = pdb.GetBytes(32);
		encryptor.IV = pdb.GetBytes(16);

		return encryptor;
	}
}