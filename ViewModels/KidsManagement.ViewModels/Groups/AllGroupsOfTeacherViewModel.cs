using System;
using System.Collections.Generic;
using System.Text;

namespace KidsManagement.ViewModels.Groups
{
    public class AllGroupsOfTeacherViewModel
    {
        public string TeacherName { get; set; }
        public ICollection<SingleGroupOfTeacherDetailsViewModel> Groups { get; set; }

    }
}
