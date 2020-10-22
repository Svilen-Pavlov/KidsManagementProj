using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CustomExtensions.Attributes
{

    [AttributeUsage(AttributeTargets.Property)]
    public class DateGreaterThanAttribute : ValidationAttribute
    {
        public DateGreaterThanAttribute(string dateToCompareToFieldName)
        {
            DateToCompareToFieldName = dateToCompareToFieldName;
        }

        private string DateToCompareToFieldName { get; set; }

        public override string FormatErrorMessage(string name)
        {
            return "End Date should be after Start Date.";
        }
        protected override ValidationResult IsValid(object objValue, ValidationContext validationContext)
        {
            var earlierDate = validationContext.ObjectType.GetProperty(DateToCompareToFieldName).GetValue(validationContext.ObjectInstance, null) as DateTime? ?? new DateTime();
            var laterDate = objValue as DateTime? ?? new DateTime();
            
            if (laterDate==new DateTime() || earlierDate==new DateTime()) return ValidationResult.Success;
            if (laterDate <= earlierDate)
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }
            return ValidationResult.Success;
        }
    }

}
