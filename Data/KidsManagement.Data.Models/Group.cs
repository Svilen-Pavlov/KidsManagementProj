using KidsManagement.Data.Models.Enums;
using KidsManagement.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using KidsManagement.Data.Models.Constants;

namespace KidsManagement.Data.Models
{
    public class Group
    {
        public Group()
        {
            this.Students = new HashSet<Student>();
        }
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(Const.entityNameMaxLen)]
        public string Name { get; set; }

        [Range(Const.entityMinCount, Const.entityMaxCount)]
        public int CurrentLessonNumber { get; set; }

        [Required]
        public AgeGroup AgeGroup { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public DayOfWeek DayOfWeek { get; set; }

        [Required]
        public TimeSpan Duration { get; set; }
        //da smenq na string ako stane fal?
        [Required]
        [Column(TypeName = "time(0)")]
        public TimeSpan StartTime { get; set; }
        //da smenq na string ako stane fal?
        [Required]
        [Column(TypeName = "time(0)")]
        public TimeSpan EndTime { get; set; }

        //stats
        public int MaxStudents { get; set; }

        //[Required] make it required tho
        public GroupStatus Status { get;set; }

        //links
        public int? TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        public int? LevelId { get; set; }
        public Level Level { get; set; }

        public virtual ICollection<Student> Students { get; set; }

        //attendance?
        //variant za vuzrast predu4/ u4ili6tna

        
        public override string ToString()
        {
            return this.Name;
        }

    }
}
