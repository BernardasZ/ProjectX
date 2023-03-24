using Domain.Resources;

namespace Domain.Exeptions;

public interface IExceptionTranslationMapper
{
    string GetErrorTranslation(IResourceManager resourceManager);
}