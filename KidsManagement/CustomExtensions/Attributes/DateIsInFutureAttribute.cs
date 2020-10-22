using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CustomExtensions.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DateIsInFutureAttribute : ValidationAttribute
    {
        public override string FormatErrorMessage(string name)
        {
            return "Date should not be a past date.";
        }

        protected override ValidationResult IsValid(object objValue,ValidationContext validationContext)
        {
            var dateValue = objValue as DateTime? ?? new DateTime();

            if (dateValue.Date < DateTime.Now.Date)
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }
            return ValidationResult.Success;
        }
    }
}
