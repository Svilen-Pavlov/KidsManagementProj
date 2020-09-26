using KidsManagement.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace KidsManagement.ViewModels.Parents
{
    public class AllSingleParentsViewModel
    {
        public int Id { get; set; }

        public string FulLName { get; set; }

        public Gender Gender { get; set; }

        public int StudentsCount { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

    }
}
