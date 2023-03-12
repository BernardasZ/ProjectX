using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using ToDoList.Api.Options;

namespace ToDoList.Api.Services.Concrete;

public class MessageService : IMessageService
{
	private readonly ILogger logger = Log.ForContext<MessageService>();
	private readonly IOptionsMonitor<OptionManager> _optionManager;

	public MessageService(IOptionsMonitor<OptionManager> optionManager)
	{
		_optionManager = optionManager;
	}

	public void SendEmail(MailMessage message)
	{
		Task.Run(() =>
		{
			try
			{
				SendEmailMessage(message);
			}
			catch (Exception e)
			{
				logger.Error(e, "Failed to send email message");
			}
		});
	}

	private void SendEmailMessage(MailMessage message)
	{
		using (var smtp = new SmtpClient())
		{
			smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
			smtp.EnableSsl = true;
			smtp.Host = _optionManager.CurrentValue.SmtpSettings.Host;
			smtp.Port = 587;
			smtp.Credentials = new NetworkCredential(
				_optionManager.CurrentValue.SmtpSettings.UserName,
				_optionManager.CurrentValue.SmtpSettings.Password);
			smtp.Send(message);
		}
	}
}