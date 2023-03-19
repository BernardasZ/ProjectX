using Api.Enums;
using System;
using System.Runtime.Serialization;

namespace Api.Exeptions;

public class GenericException : Exception
{
	public string ErrorCode { get; set; }

	public GenericException(GenericError errorEnum)
		: base()
	{
		ErrorCode = errorEnum.ToString();
	}

	public GenericException(string message)
		: base(message)
	{
	}

	protected GenericException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public GenericException()
		: base()
	{
	}
}