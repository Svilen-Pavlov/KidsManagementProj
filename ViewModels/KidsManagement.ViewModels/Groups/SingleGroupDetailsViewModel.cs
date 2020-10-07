using KidsManagement.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace KidsManagement.ViewModels.Groups
{
    public class SingleGroupDetailsViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [DisplayName("Level Name")]
        public string LevelName { get; set; }
        [DisplayName("Id")]
        public DayOfWeek DayOfWeek { get; set; }

        [DisplayName("Age Group")]
        public AgeGroup AgeGroup { get; set; }
        [DisplayName("Start Time")]
        public string StartTime { get; set; }

        [DisplayName("Teacher")]
        public string TeacherName { get; set; }

        [DisplayName("Current Students")]
        public int StudentsCount { get; set; }
        [DisplayName("Maximum Students")]
        public int MaxStudentsCount { get; set; }
        [DisplayName("Active Status")]
        public GroupActiveStatus ActiveStatus { get; set; }

        public bool ActiveGroup { get; set; } //specifically for Add/Change Method
    }
}
