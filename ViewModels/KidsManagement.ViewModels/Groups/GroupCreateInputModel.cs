using KidsManagement.Data.Models;
using KidsManagement.Data.Models.Enums;
using KidsManagement.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace KidsManagement.ViewModels.Groups
{
    public class GroupCreateInputModel
    {
        [Required]
        [MaxLength(Const.entityNameMaxLen)]
        public string Name { get; set; }

        [Range(Const.entityMinCount, Const.entityMaxCount)]
        public int CurrentLessonNumber { get; set; }

        [Required]
        public AgeGroup AgeGroup { get; set; } //AgeGroup enum

        [Required]
        public DateTime StartDate { get; set; } //DateTime

        [Required]
        public DateTime EndDate { get; set; } //DateTime

        [Required]
        public DayOfWeek DayOfWeek { get; set; } //DayOfWeek

        [Required]
        public TimeSpan Duration { get; set; } //TimeSpan
        
        [Required]
        [Column(TypeName = "time(0)")]
        public TimeSpan StartTime { get; set; } //TimeSpan
      
        [Required]
        [Column(TypeName = "time(0)")]
        public TimeSpan EndTime { get; set; } //TimeSpan


        [Required]
        public int TeacherId { get; set; }
    
        [Required]
        public int LevelId { get; set; }
   
    }
}
