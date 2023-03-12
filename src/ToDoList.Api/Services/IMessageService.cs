using System.Net.Mail;

namespace ToDoList.Api.Services;

public interface IMessageService
{
	void SendEmail(MailMessage message);
}