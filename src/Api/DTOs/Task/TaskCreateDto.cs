using Api.Validators;
using Domain.Enums;

namespace Api.DTOs.Task;

public class TaskCreateDto : BaseValidatableObject, IDtoBase
{
	public int UserId { get; set; }

	public string Name { get; set; }

	public TaskStatus Status { get; set; }

	protected override IBaseValidator<ITaskValidator> Validate() => new TaskValidator()
		.ValidateId(UserId, nameof(UserId))
		.ValidateString(Name, nameof(Name));
}