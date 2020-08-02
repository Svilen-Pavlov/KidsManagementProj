using KidsManagement.Data.Models;
using KidsManagement.Data.Models.Enums;
using KidsManagement.ViewModels.Groups;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace KidsManagement.ViewModels.Teachers
{
    public class TeacherDetailsViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public decimal Salary { get; set; }
        public string HiringDate { get; set; }
        public string DismissalDate { get; set; } 

        public string ProfilePicURI { get; set; }
        public IEnumerable<Level> QualifiedLevels { get; set; }

        public List<GroupSelectionViewModel> Groups { get; set; }

        public string FullName => string.Format("{0} {1}", FirstName,  LastName);

        //user properties
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

    }
}
