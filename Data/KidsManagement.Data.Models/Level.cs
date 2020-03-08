﻿using KidsManagement.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KidsManagement.Data.Models
{
    public class Level
    {

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(Const.entityNameMaxLen)]
        public string Name { get; set; }
        
        [Required]
        [MaxLength(Const.textMaxLen)]
        public string Description { get; set; }

        [Required]
        [MaxLength(Const.textMaxLen)]
        public string StudyMaterialsDescription { get; set; }

        
        public IEnumerable<LevelTeacher> EligibleTeachers { get; set; }
        public IEnumerable<Group> Groups { get; set; }



        //min 1, purvo vtoro nivo
        //[Required]
        //[Range(Const.entityMinCount,Const.entityMaxCount)]
        //public int CurriculumNumber { get; set; }

        //[Required]
        //public int SubjectId { get; set; }
        //public Subject Subject { get; set; }
    }
}