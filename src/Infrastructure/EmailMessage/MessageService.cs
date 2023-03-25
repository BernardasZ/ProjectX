using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Application.Messages;
using Infrastructure.EmailMessage.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.EmailMessage;

internal class MessageService : IMessageService
{
	private readonly ILogger<MessageService> _logger;
	private readonly IOptionsMonitor<SmtpSettings> _smtpSettings;

	public MessageService(
		IOptionsMonitor<SmtpSettings> smtpSettings,
		ILogger<MessageService> logger)
	{
		_smtpSettings = smtpSettings;
		_logger = logger;
	}

	public void SendEmailMessage(string to, string subject, string body)
	{
		var message = new MailMessage(_smtpSettings.CurrentValue.Sender, to, subject, body)
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
			smtp.Host = _smtpSettings.CurrentValue.Host;
			smtp.Port = 587;
			smtp.Credentials = new NetworkCredential(
				_smtpSettings.CurrentValue.UserName,
				_smtpSettings.CurrentValue.Password);
			smtp.Send(message);
		}
	}
}