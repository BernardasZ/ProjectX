using ToDoList.Api.Exeptions;
using TaskData = DataModel.Entities.ProjectX.Task;

namespace ToDoList.Api.Helpers
{
	public interface ITaskServiceValidationHelper
	{
		void ValidateTaskData(TaskData model);
	}

	public class TaskServiceValidationHelper : ITaskServiceValidationHelper
	{
		public TaskServiceValidationHelper()
		{

		}

		public void ValidateTaskData(TaskData model)
		{
			if (model == null)
				throw new GenericException(Enums.GenericErrorEnum.TaskDoesNotExist);
		}
	}
}
