using System.Runtime.Serialization;
using Application.Database.Enums;
using Domain.Exeptions;

namespace Application.Database.Exceptions;

public class RepositoryBaseException : ExceptionBase<RepositoryErrorCodes>
{
	public RepositoryBaseException(RepositoryErrorCodes errorCode)
		: base(errorCode)
	{
	}

	public RepositoryBaseException(RepositoryErrorCodes errorCode, int statusCode)
		: base(errorCode, statusCode)
	{
	}

	public RepositoryBaseException(
		RepositoryErrorCodes errorCode,
		string message = null,
		Exception innerException = null,
		int? statusCode = null)
		: base(errorCode, message, innerException, statusCode)
	{
	}

	protected RepositoryBaseException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}