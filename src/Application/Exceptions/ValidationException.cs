using System.Runtime.Serialization;
using Application.Exceptions.Enums;
using Domain.Exeptions;

namespace Application.Exceptions;

public class ValidationException : ExceptionBase<ValidationErrorCodes>
{
	public ValidationException(ValidationErrorCodes errorCode)
		: base(errorCode)
	{
	}

	public ValidationException(ValidationErrorCodes errorCode, int statusCode) : base(errorCode, statusCode)
	{
	}

	public ValidationException(ValidationErrorCodes errorCode, string message = null, Exception innerException = null)
		: base(errorCode, message, innerException)
	{
	}

	public ValidationException(
		ValidationErrorCodes errorCode,
		string message = null,
		Exception innerException = null,
		int? statusCode = null)
		: base(errorCode, message, innerException, statusCode)
	{
	}

	protected ValidationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}