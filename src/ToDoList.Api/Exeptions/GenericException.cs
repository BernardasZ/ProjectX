using System;
using System.Runtime.Serialization;
using ToDoList.Api.Enums;

namespace ToDoList.Api.Exeptions;

public class GenericException : Exception
{
	public string ErrorCode { get; set; }

	public GenericException(GenericErrorEnum errorEnum)
		: base()
	{
		ErrorCode = errorEnum.ToString();
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
