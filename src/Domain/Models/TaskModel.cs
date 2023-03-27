using TaskStatus = Domain.Enums.TaskStatus;

namespace Domain.Models;

public class TaskModel : ModelBase
{
	public string Title { get; set; }

	public string Description { get; set; }

	public TaskStatus Status { get; set; }

	public virtual UserModel User { get; set; }
}