using Domain.Enums;
using Domain.Validators;

namespace Api.DTOs.Task;

public class TaskCreateDto : BaseValidatableObject
{
	public int UserId { get; set; }

	public string Title { get; set; }

	public string Description { get; set; }

	public TaskStatus Status { get; set; }

	protected override IBaseValidator<ITaskValidator> Validate() => new TaskValidator()
		.ValidateId(UserId, nameof(UserId))
		.ValidateString(Title, nameof(Title))
		.ValidateString(Description, nameof(Description));
}