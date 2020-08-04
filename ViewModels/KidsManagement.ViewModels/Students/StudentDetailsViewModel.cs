using KidsManagement.Data.Models.Enums;
using KidsManagement.ViewModels.Parents;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KidsManagement.ViewModels.Students
{
    public class StudentDetailsViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string FullName => string.Format("{0} {1} {2}", FirstName, MiddleName, LastName);

        public Gender Gender { get; set; }

        public int Age { get; set; }

        public string BirthDate { get; set; }

        public GradeLevel Grade { get; set; }
        public AgeGroup AgeGroup { get; set; }

        public StudentStatus Status { get; set; }
        public int? GroupId { get; set; }

        public string GroupName { get; set; }
        public string ProfilePicURI { get; set; }

        public IEnumerable<ParentsSelectionViewModel> Parents { get; set; }
    }
}
