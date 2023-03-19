using Persistence.Entities.ProjectX;

namespace Api.Helpers;

public interface ITaskServiceValidationHelper
{
	void CheckIfNotNull(Task model);
}