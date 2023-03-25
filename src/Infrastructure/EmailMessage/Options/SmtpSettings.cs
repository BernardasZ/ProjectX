namespace Infrastructure.EmailMessage.Options;

public class SmtpSettings
{
	public const string SelectionName = nameof(SmtpSettings);

	public string Host { get; set; }

	public string UserName { get; set; }

	public string Password { get; set; }

	public string Sender { get; set; }
}