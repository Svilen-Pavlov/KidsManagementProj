using KidsManagement.Data;
using KidsManagement.Data.Models.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KidsManagement.Data.Models
{
    public class Level
    {
        public Level()
        {
            this.EligibleTeachers = new HashSet<LevelTeacher>();
            this.Groups = new HashSet<Group>();
        }

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

        
        public virtual ICollection<LevelTeacher> EligibleTeachers { get; set; }
        public virtual ICollection<Group> Groups { get; set; }



        //min 1, purvo vtoro nivo
        //[Required]
        //[Range(Const.entityMinCount,Const.entityMaxCount)]
        //public int CurriculumNumber { get; set; }

        //[Required]
        //public int SubjectId { get; set; }
        //public Subject Subject { get; set; }
    }
}