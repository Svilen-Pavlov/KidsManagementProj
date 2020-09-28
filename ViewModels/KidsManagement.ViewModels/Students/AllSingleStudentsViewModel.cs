using KidsManagement.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace KidsManagement.ViewModels.Students
{
    public class AllSingleStudentsViewModel
    {
        public int Id { get; set; }

        public string FullName {get; set;}

        public Gender Gender { get; set; }

        public int Age { get; set; }
        public StudentStatus Status { get; set; }

        public string GroupName { get; set; }
    }
}
