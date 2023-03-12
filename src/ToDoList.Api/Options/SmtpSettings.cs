namespace ToDoList.Api.Options;

public class SmtpSettings
{
	public string Host { get; set; }
	public string UserName { get; set; }
	public string Password { get; set; }
	public string Sender { get; set; }
}
