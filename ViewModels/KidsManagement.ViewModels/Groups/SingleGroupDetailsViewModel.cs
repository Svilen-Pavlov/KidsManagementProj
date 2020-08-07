using KidsManagement.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace KidsManagement.ViewModels.Groups
{
    public class SingleGroupDetailsViewModel
    {
        [DisplayName("Id")]
        public int Id { get; set; }

        public string Name { get; set; }

        public string LevelName { get; set; }
        public DayOfWeek DayOfWeek { get; set; }

        public AgeGroup AgeGroup { get; set; }
        public string StartTime { get; set; }

        public string TeacherName { get; set; }

        public int StudentsCount { get; set; }
        public int MaxStudentsCount { get; set; }
        //public double Compatibility { get; set; }

        public bool ActiveGroup { get; set; } //specifically for Add/Change Method
    }
}
