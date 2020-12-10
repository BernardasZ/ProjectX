using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.Api.Validators
{
	public class TaskValidator
	{
        private ICollection<ValidationResult> results = null;

        public TaskValidator()
        {
            this.results = new List<ValidationResult>();
        }

        public void ValidateName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                results.Add(new ValidationResult("Invalid task name"));
        }

        public void ValidateUserId(int value)
        {
            if (value <= 0)
                results.Add(new ValidationResult("Invalid user id"));
        }

        public void ValidateTaskId(int value)
        {
            if (value <= 0)
                results.Add(new ValidationResult("Invalid task id"));
        }

        public IEnumerable<ValidationResult> GetValidationResults()
        {
            return this.results;
        }
    }
}
