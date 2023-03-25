﻿using System.Runtime.Serialization;
using Application.Exceptions.Enums;
using Domain.Exeptions;

namespace Application.Exceptions;

public class ValidationException : ExceptionBase<ValidationErrorCodes>
{
	public ValidationException(ValidationErrorCodes errorCode)
		: base(errorCode)
	{
	}

	public ValidationException(ValidationErrorCodes errorCode, string message = null, Exception innerException = null)
		: base(errorCode, message, innerException)
	{
	}

	protected ValidationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}