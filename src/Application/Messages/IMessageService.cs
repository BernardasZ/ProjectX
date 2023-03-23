namespace Application.Messages;

public interface IMessageService
{
	void SendEmailMessage(string to, string subject, string body);
}