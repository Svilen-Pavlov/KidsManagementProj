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
        public DateTime HiringDate { get; set; }
        public DateTime? DismissalDate { get; set; } //da ne pokazva null a smislen string

        public string ProfilePicURI { get; set; }
        public IEnumerable<Level> QualifiedLevels { get; set; }

        public List<GroupSelectionViewModel> Groups { get; set; }


        public string FullName => string.Format("{0} {1}", FirstName,  LastName);

    }
}
