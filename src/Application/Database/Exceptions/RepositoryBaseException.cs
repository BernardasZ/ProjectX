using Application.Database.Enums;
using Domain.Exeptions;
using System.Runtime.Serialization;

namespace Application.Database.Exceptions;

public class RepositoryBaseException : ExceptionBase<RepositoryErrorCodes>
{
	public RepositoryBaseException(RepositoryErrorCodes errorCode)
		: base(errorCode)
	{
	}

	public RepositoryBaseException(RepositoryErrorCodes errorCode, string message = null, Exception innerException = null)
		: base(errorCode, message, innerException)
	{
	}

	protected RepositoryBaseException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}