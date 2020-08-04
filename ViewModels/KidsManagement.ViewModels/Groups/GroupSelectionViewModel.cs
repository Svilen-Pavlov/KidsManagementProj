using System;
using System.Collections.Generic;
using System.Text;

namespace KidsManagement.ViewModels.Groups
{
    public class GroupSelectionViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        //public string TeacherName { get; set; }
        //public string AgeGroup { get; set; }
        //public string DayOfWeek { get; set; }

        public bool Selected { get; set; } //for checkbox forms specifically
    }
}
