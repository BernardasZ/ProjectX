using System.Net.Mail;

namespace Api.Services;

public interface IMessageService
{
	void SendEmail(MailMessage message);
}