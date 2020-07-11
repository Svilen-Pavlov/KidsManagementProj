using KidsManagement.Data.Models;
using KidsManagement.Data.Models.Enums;
using KidsManagement.ViewModels.Groups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KidsManagement.ViewModels.Teachers
{
    public class TeachersListDetailsViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //public Gender Gender { get; set; }
        //public decimal Salary { get; set; }
        //public DateTime HiringDate { get; set; }
        //public DateTime? DismissalDate { get; set; }

        public int GroupsCount => this.Groups.ToArray().Count();
        public double Capacity { get; set; }

        public double Efficiency { get; set; }
        public IEnumerable<string> Groups { get; set; }

        //public IEnumerable<Level> QualifiedLevels { get; set; }
    }
}
