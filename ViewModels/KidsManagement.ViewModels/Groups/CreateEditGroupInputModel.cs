using KidsManagement.Data.Models.Constants;
using KidsManagement.Data.Models.Enums;
using KidsManagement.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KidsManagement.ViewModels.Teachers;
using KidsManagement.ViewModels.Levels;
using System.ComponentModel;
using CustomExtensions.Attributes;

namespace KidsManagement.ViewModels.Groups
{
    public class CreateEditGroupInputModel
    {
        public int Id { get; set; } //only for edit

        [Required]
        [MaxLength(Constants.entityNameMaxLen)]
        public string Name { get; set; }

        //[Range(Const.entityMinCount, Const.entityMaxCount)]

        [DisplayName("Age Group")]
        [Required]
        public AgeGroup AgeGroup { get; set; } //AgeGroup enum

        [Required]
        [DisplayName("Start Date")]
        public DateTime StartDate { get; set; } //DateTime

        [Required]
        [DateGreaterThan("StartDate")]
        [DisplayName("End Date")]
        public DateTime EndDate { get; set; } //DateTime

        [Required]
        [DisplayName("Weekday")]
        public DayOfWeek DayOfWeek { get; set; } //DayOfWeek

        [Required]
        [DisplayName("Start Time")]
        [Column(TypeName = "time(0)")]
        public TimeSpan StartTime { get; set; } //TimeSpan

        [DisplayName("End Time")]
        [Column(TypeName = "time(0)")]
        public TimeSpan EndTime { get; set; } //TimeSpan

        [Required]
        [Column(TypeName = "time(0)")] //idk if I need this column
        public TimeSpan Duration { get; set; } //TimeSpan

        [Display(Name = "Teacher")]
        public int TeacherId { get; set; }

        [Required]
        [Display(Name = "Level")]
        public int LevelId { get; set; }

        public IEnumerable<TeacherSelectionViewModel> Teachers { get; set; }
        public IEnumerable<LevelSelectionViewModel> Levels { get; set; }


    }
}
