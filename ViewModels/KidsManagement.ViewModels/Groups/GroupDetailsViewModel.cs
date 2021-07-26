using KidsManagement.Data;
using KidsManagement.Data.Models;
using KidsManagement.Data.Models.Enums;
using KidsManagement.ViewModels.Students;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace KidsManagement.ViewModels.Groups
{
    public class GroupDetailsViewModel
    {
        
        public int Id { get; set; }
      
        public string Name { get; set; }

        [DisplayName("Lesson #")]
        public int CurrentLessonNumber { get; set; }

        [DisplayName("Age Group")]
        public string AgeGroup { get; set; }

        [DisplayName("StartDate")]
        public string StartDate { get; set; }

        [DisplayName("End Date")]
        public string EndDate { get; set; }

        [DisplayName("Weekday")]
        public DayOfWeek DayOfWeek { get; set; }

        [DisplayName("Duration (in hours)")]
        public string Duration { get; set; }
        [DisplayName("Start Time")]
        public string StartTime { get; set; }
        [DisplayName("End Time")]
        public string EndTime { get; set; }
        [DisplayName("Active Status")]
        public GroupActiveStatus ActiveStatus { get; set; }

        public int TeacherId { get; set; }
        [DisplayName("Teacher")]
        public string TeacherName { get; set; }

        public int LevelId { get; set; }
        
        [DisplayName("Level")]
        public string LevelName { get; set; }

        [DisplayName("Max Students")]
        public int MaxStudents { get; set; }
        public IEnumerable<AllSingleStudentsViewModel> Students { get; set; }

        public List<StudentSelectionViewModel> StudentsForSelection { get; set; }

    }
}
