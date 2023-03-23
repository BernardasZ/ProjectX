using Api.Validators;
using Domain.Enums;

namespace Api.DTOs.Task;

public class TaskDto : BaseValidatableObject, IDtoBase
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