using KidsManagement.Data.Models;
using KidsManagement.Data.Models.Enums;
using KidsManagement.ViewModels.Students;
using System;
using System.Collections.Generic;

namespace KidsManagement.ViewModels.Groups
{
    public class SingleGroupOfTeacherDetailsViewModel
    {
        //general
        public int Id { get; set; }

        public string Name { get; set; }

        public int CurrentLessonNumber { get; set; }

        public AgeGroup AgeGroup { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DayOfWeek DayOfWeek { get; set; }

        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public TimeSpan Duration { get; set; }

        //links
        public int TeacherId { get; set; }
        public int LevelId { get; set; }


        //stats
        public double Capacity { get; set; } 
        public double Efficiency { get; set; }

    }
}
