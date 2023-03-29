using System.Globalization;
using System.Runtime.Serialization;
using Domain.Resources;
using Microsoft.AspNetCore.Http;

namespace Domain.Exeptions;

public abstract class ExceptionBase<TEnum> : Exception, IExceptionTranslationMapper
	where TEnum : Enum
{
	private readonly TEnum _errorCode;
	private readonly int _statusCode = StatusCodes.Status400BadRequest;

	public ExceptionBase(TEnum errorCode)
		: base() => _errorCode = errorCode;

	public ExceptionBase(TEnum errorCode, int statusCode)
	: base()
	{
		_errorCode = errorCode;
		_statusCode = statusCode;
	}

	public ExceptionBase(
		TEnum errorCode,
		string message = null,
		Exception innerException = null,
		int? statusCode = null)
		: base(message, innerException)
	{
		_errorCode = errorCode;
		_statusCode = statusCode ?? _statusCode;
	}

	protected ExceptionBase(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public virtual string GetErrorTranslation(IResourceManager resourceManager) =>
		resourceManager.GetString($"{GetType().Name}_{_errorCode}", CultureInfo.CurrentCulture);

	public virtual int GetStatusCode() => _statusCode;

	public virtual string GetErrorCode() => _errorCode.ToString();
}