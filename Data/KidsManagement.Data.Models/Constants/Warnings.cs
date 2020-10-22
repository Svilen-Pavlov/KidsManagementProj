using System;
using System.Collections.Generic;
using System.Text;

namespace KidsManagement.Data.Models.Constants
{
    public static class Warnings
    {
        public const string CreatHumanName = "Name must be between 2 and 40 non-number characters."; // :(
        public const string CreatEntityName = "Name must be between 2 and 30 characters."; // :(
        public const string RequiredBirthDate = "You must select a date of birth."; 
        public const string CreateEmail = "Invalid email address"; 
        public const string CreatePhone = "Invalid phone number"; 
        public const string GroupDuration = "Duration must be between 15 minutes and 3 hours."; 
        public const string SalaryPositive = "Salary must be a positive number."; 
        
    }
}
