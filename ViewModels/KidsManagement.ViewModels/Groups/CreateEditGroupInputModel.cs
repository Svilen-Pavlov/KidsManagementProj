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
        [DisplayName("First Name")]
        [RegularExpression(Constants.entityNamesRegex, ErrorMessage = Warnings.CreatEntityName)]
        public string Name { get; set; }

        [DisplayName("Age Group")]
        [Required]
        public AgeGroup AgeGroup { get; set; } //AgeGroup enum

        [Required]
        [DisplayName("Start Date")]
        public DateTime? StartDate { get; set; } //DateTime

        [Required]
        [DisplayName("End Date")]
        [DateIsInFuture]
        [DateGreaterThan("StartDate")]
        public DateTime? EndDate { get; set; } //DateTime

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
        [Range(Constants.groupMinDuration,Constants.groupMaxDuration,ErrorMessage =Warnings.GroupDuration)] 
        [DisplayName("Duration (in hours)")]

        public int Duration { get; set; } //TimeSpan

        [Display(Name = "Teacher")]
        public int TeacherId { get; set; }

        [Required]
        [Display(Name = "Level")]
        public int LevelId { get; set; }

        public IEnumerable<TeacherSelectionViewModel> Teachers { get; set; }
        public IEnumerable<LevelSelectionViewModel> Levels { get; set; }


    }
}
