using KidsManagement.Data;
using KidsManagement.Data.Models;
using KidsManagement.Data.Models.Enums;
using KidsManagement.ViewModels.Students;
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

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public DayOfWeek DayOfWeek { get; set; }

        public string Duration { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }


        public int TeacherId { get; set; }
        public string TeacherName { get; set; }

        public int LevelId { get; set; }
        public string LevelName { get; set; }

        public int MaxStudents { get; set; }
        public IEnumerable<AllSingleStudentsViewModel> Students { get; set; }

    }
}
