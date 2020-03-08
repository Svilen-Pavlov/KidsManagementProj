using KidsManagement.Data;
using KidsManagement.Data.Models;
using KidsManagement.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace KidsManagement.ViewModels.Groups
{
    public class GroupDetailsViewModel
    {
        
        public int Id { get; set; }
      
        public string Name { get; set; }

        public int CurrentLessonNumber { get; set; }

        public AgeGroup AgeGroup { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DayOfWeek DayOfWeek { get; set; }

        public TimeSpan Duration { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }


        public int TeacherId { get; set; }
        public string TeacherName { get; set; }

        public int LevelId { get; set; }
        public string LevelName { get; set; }

        public IEnumerable<Student> Students { get; set; }

    }
}
