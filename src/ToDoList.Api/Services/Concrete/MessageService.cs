using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using ToDoList.Api.Helpers;

namespace ToDoList.Api.Services.Concrete
{
	public class MessageService : IMessageService
	{
		private readonly IOptionsMonitor<OptionManager> optionManager;
		public MessageService(IOptionsMonitor<OptionManager> optionManager)
		{
			this.optionManager = optionManager;
		}

		public async void SendEmailAsync(MailMessage message)
		{
			Task.Run(() =>
			{
				try
				{
					using (var smtp = new SmtpClient())
					{
						smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
						smtp.EnableSsl = true;
						smtp.Host = optionManager.CurrentValue.SmtpSettings.Host;
						smtp.Port = 587;
						smtp.Credentials = new NetworkCredential(optionManager.CurrentValue.SmtpSettings.UserName, optionManager.CurrentValue.SmtpSettings.Password);
						smtp.Send(message);
					}
				}
				catch (Exception)
				{
					//Log error
				}
			});
		}
	}
}
