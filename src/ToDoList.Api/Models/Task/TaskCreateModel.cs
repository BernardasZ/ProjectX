using DataModel.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Api.Validators;

namespace ToDoList.Api.Models.Task
{
	public class TaskCreateModel : IValidatableObject
    {
        public int UserId { get; set; }
        public string TaskName { get; set; }
        public TaskStatusEnum Status { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            TaskValidator validator = new TaskValidator();

            validator.ValidateUserId(UserId);
            validator.ValidateName(TaskName);

            return validator.GetValidationResults();
        }
    }
}