using Application.Messages;
using Application.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Infrastructure.EmailMessage;

internal class MessageService : IMessageService
{
	private readonly ILogger<MessageService> _logger;
	private readonly IOptionsMonitor<Configuration> _configuration;

	public MessageService(
		IOptionsMonitor<Configuration> configuration,
		ILogger<MessageService> logger)
	{
		_configuration = configuration;
		_logger = logger;
	}

	public void SendEmailMessage(string to, string subject, string body)
	{
		var message = new MailMessage(_configuration.CurrentValue.SmtpSettings.Sender, to, subject, body)
		{
			IsBodyHtml = true
		};

		Task.Run(() =>
		{
			try
			{
				SendEmailMessage(message);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to send email message");
			}
		});
	}

	private void SendEmailMessage(MailMessage message)
	{
		using (var smtp = new SmtpClient())
		{
			smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
			smtp.EnableSsl = true;
			smtp.Host = _configuration.CurrentValue.SmtpSettings.Host;
			smtp.Port = 587;
			smtp.Credentials = new NetworkCredential(
				_configuration.CurrentValue.SmtpSettings.UserName,
				_configuration.CurrentValue.SmtpSettings.Password);
			smtp.Send(message);
		}
	}
}