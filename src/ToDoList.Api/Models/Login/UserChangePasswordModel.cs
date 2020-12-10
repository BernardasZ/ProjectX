using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ToDoList.Api.Validators;

namespace ToDoList.Api.Models.Login
{
	public class UserChangePasswordModel : IValidatableObject
	{
		public string UserEmail { get; set; }
		public string NewPassword { get; set; }
        public string OldPassword { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            UserValidator validator = new UserValidator();

            validator.ValidateEmail(UserEmail);
            validator.ValidateNewPassword(NewPassword);
            validator.ValidateOldPassword(OldPassword);

            return validator.GetValidationResults();
        }
    }
}
