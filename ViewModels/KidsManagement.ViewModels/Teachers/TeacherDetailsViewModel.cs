using KidsManagement.Data.Models;
using KidsManagement.Data.Models.Enums;
using KidsManagement.ViewModels.Groups;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace KidsManagement.ViewModels.Teachers
{
    public class TeacherDetailsViewModel
    {
        public int Id { get; set; }
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public string Salary { get; set; }
        [DisplayName("Hiring Date")]
        public string HiringDate { get; set; }
        [DisplayName("Dismissal Date")]
        public string DismissalDate { get; set; } 
        public TeacherStatus Status { get; set; } 
        [DisplayName("Profile Picture")]
        public string ProfilePicURI { get; set; }
        public IEnumerable<Level> QualifiedLevels { get; set; }

        public List<GroupSelectionViewModel> Groups { get; set; }

        public string FullName => string.Format("{0} {1}", FirstName,  LastName);

        //user properties
        public string Username { get; set; }
        public string Email { get; set; }
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

    }
}
