using KidsManagement.Data.Models.Constants;
using KidsManagement.Data.Models.Enums;
using KidsManagement.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KidsManagement.ViewModels.Teachers;
using KidsManagement.ViewModels.Levels;

namespace KidsManagement.ViewModels.Groups
{
    public class CreateGroupInputModel
    {
        [Required]
        [MaxLength(Const.entityNameMaxLen)]
        public string Name { get; set; }

        //[Range(Const.entityMinCount, Const.entityMaxCount)]

        [Required]
        public AgeGroup AgeGroup { get; set; } //AgeGroup enum

        [Required]
        public DateTime StartDate { get; set; } //DateTime

        [Required]
        public DateTime EndDate { get; set; } //DateTime

        [Required]
        public DayOfWeek DayOfWeek { get; set; } //DayOfWeek

        [Required]
        [Column(TypeName = "time(0)")]
        public TimeSpan StartTime { get; set; } //TimeSpan
      
        [Required]
        [Column(TypeName = "time(0)")]
        public TimeSpan EndTime { get; set; } //TimeSpan

        [Display(Name="Teacher")]
        public int TeacherId { get; set; }

        [Required]
        [Display(Name = "Level")]
        public int LevelId { get; set; }
   
        public IEnumerable<TeacherSelectionViewModel> Teachers { get; set; }
        public IEnumerable<LevelSelectionViewModel> Levels { get; set; }


    }
}
