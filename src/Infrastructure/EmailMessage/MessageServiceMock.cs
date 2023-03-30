using Application.Messages;
using Microsoft.Extensions.Logging;

namespace Infrastructure.EmailMessage;

internal class MessageServiceMock : IMessageService
{
	private readonly ILogger<MessageService> _logger;

	public MessageServiceMock(ILogger<MessageService> logger) => _logger = logger;

	public void SendEmailMessage(string to, string subject, string body) =>
		_logger.LogInformation("Email message: {@emailMessageObject}", new { to, subject, body });
}