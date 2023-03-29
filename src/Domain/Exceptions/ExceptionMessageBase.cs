namespace Domain.Exceptions;

[Serializable]
public class ExceptionMessageBase
{
	public string Code { get; set; }

	public string Message { get; set; }
}