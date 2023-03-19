using Api.Validators;
using Persistence.Enums;

namespace Api.Models.Task;

public class TaskCreateModel : BaseValidatableObject, IBaseModel
{
	public int UserId { get; set; }

	public string Name { get; set; }

	public TaskStatus Status { get; set; }

	protected override IBaseValidator<ITaskValidator> Validate() => new TaskValidator()
		.ValidateId(UserId, nameof(UserId))
		.ValidateString(Name, nameof(Name));
}