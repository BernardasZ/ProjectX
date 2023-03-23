using Domain.Enums;
using Domain.Validators;

namespace Api.DTOs.Task;

public class TaskUpdateDto : BaseValidatableObject
{
	public int Id { get; set; }

	public int UserId { get; set; }

	public string Name { get; set; }

	public TaskStatus Status { get; set; }

	protected override IBaseValidator<ITaskValidator> Validate() => new TaskValidator()
		.ValidateId(Id, nameof(Id))
		.ValidateId(UserId, nameof(UserId))
		.ValidateString(Name, nameof(Name));
}