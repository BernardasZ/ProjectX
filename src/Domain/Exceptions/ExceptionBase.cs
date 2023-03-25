using System.Globalization;
using System.Runtime.Serialization;
using Domain.Resources;

namespace Domain.Exeptions;

public abstract class ExceptionBase<TEnum> : Exception, IExceptionTranslationMapper
	where TEnum : Enum
{
	private readonly TEnum ErrorCode;

	public ExceptionBase(TEnum errorCode)
		: base() => ErrorCode = errorCode;

	public ExceptionBase(
		TEnum errorCode,
		string message = null,
		Exception innerException = null)
		: base(message, innerException) => ErrorCode = errorCode;

	protected ExceptionBase(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public virtual string GetErrorTranslation(IResourceManager resourceManager) =>
		resourceManager.GetString($"{GetType().Name}_{ErrorCode}", CultureInfo.CurrentCulture);
}