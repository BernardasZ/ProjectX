namespace Application.Options;

public class CryptographySettings
{
	public const string SelectionName = nameof(CryptographySettings);

	public string AesKey { get; set; }

	public string AlgorithmIV { get; set; }
}