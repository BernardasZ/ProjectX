using Domain.Resources;

namespace Application.Exeptions;

public interface IExceptionTranslationMapper
{
	string GetErrorTranslation(IResourceManager resourceManager);
}