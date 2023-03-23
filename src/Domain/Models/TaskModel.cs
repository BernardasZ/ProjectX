using TaskStatus = Domain.Enums.TaskStatus;

namespace Domain.Models;

public class TaskModel : ModelBase
{
	public string Name { get; set; }

	public TaskStatus Status { get; set; }

	public virtual UserModel User { get; set; }
}