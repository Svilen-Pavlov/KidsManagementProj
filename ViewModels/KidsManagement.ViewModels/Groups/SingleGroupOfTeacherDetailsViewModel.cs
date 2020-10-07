using KidsManagement.Data.Models;
using KidsManagement.Data.Models.Enums;
using KidsManagement.ViewModels.Students;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace KidsManagement.ViewModels.Groups
{
    public class SingleGroupOfTeacherDetailsViewModel
    {

        public int Id { get; set; }

        public string Name { get; set; }

        [DisplayName("Lesson #")]
        public int CurrentLessonNumber { get; set; }

        [DisplayName("Age Group")]
        public AgeGroup AgeGroup { get; set; }

        [DisplayName("Start Date")]
        public DateTime StartDate { get; set; }

        [DisplayName("End Date")]
        public DateTime EndDate { get; set; }

        [DisplayName("Weekday")]
        public DayOfWeek DayOfWeek { get; set; }


        [DisplayName("Start Time")]
        public TimeSpan StartTime { get; set; }
        [DisplayName("End Time")]
        public TimeSpan EndTime { get; set; }
        public TimeSpan Duration { get; set; }
        [DisplayName("Size Status")]
        public GroupStatus Status { get; set; }
        [DisplayName("Active Status")]
        public GroupActiveStatus ActiveStatus { get; set; }

        //links
        public int TeacherId { get; set; }
        public int LevelId { get; set; }


        //stats
        public double Capacity { get; set; }
        public double Efficiency { get; set; }

    }
}
