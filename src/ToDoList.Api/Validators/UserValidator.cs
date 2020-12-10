using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ToDoList.Api.Validators
{
    public class UserValidator
    {
        private ICollection<ValidationResult> results = null;
        private string emailRegex = @"^[a-z0-9][-a-z0-9.!#$%&'*+-=?^_`{|}~\/]+@([-a-z0-9]+\.)+[a-z]{2,5}$";

		public UserValidator()
		{
            this.results = new List<ValidationResult>();
        }

        public void ValidateName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                results.Add(new ValidationResult("Invalid user name"));
        }

        public void ValidateEmail(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || !(new Regex(emailRegex).IsMatch(value)))
                results.Add(new ValidationResult("Invalid user email"));
        }

        public void ValidatePassword(string value)
        {
            ValidatePassword(value, "Current");
        }

        public void ValidateNewPassword(string value)
        {
            ValidatePassword(value, "New");
        }

        public void ValidateOldPassword(string value)
        {
            ValidatePassword(value, "Old");
        }

        public void ValidateToken(string value)
		{
            if (string.IsNullOrWhiteSpace(value))
                results.Add(new ValidationResult($"Invalid token"));
        }

        public void ValidateUserId(int value)
        {
            if (value <= 0)
                results.Add(new ValidationResult("Invalid user id"));
        }

        public void ValidateEmailOrName(string email, string name)
        {
            if ((string.IsNullOrWhiteSpace(email) || !(new Regex(emailRegex).IsMatch(email))) && string.IsNullOrWhiteSpace(name))
            {
                results.Add(new ValidationResult("User name or email must be filled"));
            }
        }

        public IEnumerable<ValidationResult> GetValidationResults()
		{
            return this.results;
		}
        
        private void ValidatePassword(string value, string identifier)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                results.Add(new ValidationResult(string.Format("{0} password is invalid", identifier)));
            }
            else if (value.Length < 12)
            {
                results.Add(new ValidationResult(string.Format("{0} password is too short", identifier)));
            }
        }
    }
}
