using KidsManagement.Data;
using KidsManagement.Data.Models.Constants;
using KidsManagement.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace KidsManagement.Data.Models
{
    public class Teacher
    {

        [Key]
        public int Id { get; set; }
        [Required]
        [MinLength(Const.humanNameMinLen), MaxLength(Const.humanNameMaxLen)]
        public string FirstName { get; set; }
        [Required]
        [MinLength(Const.humanNameMinLen), MaxLength(Const.humanNameMaxLen)]
        public string LastName { get; set; }
        [NotMapped]
        public string FullName => string.Format("{0} {1}", FirstName, LastName);
        [Required]
        public Gender Gender { get; set; }
        [Required]
        public decimal Salary { get; set; }
        [Required]
        public DateTime HiringDate { get; set; }
        public DateTime? DismissalDate { get; set; }
        public string ProfilePicURI { get; set; }

        //[Required] this will break old teachers
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
        public virtual ICollection<Comment> StudentComments { get; set; }
        //many to many
        public virtual ICollection<LevelTeacher> QualifiedLevels { get; set; }

        //public IEnumerable<DayOfWeek> WorkDays { get; set; }
        //[Required]
        //public int TotalStudentsCount { get; set; } FOR SERVICES!

        public override string ToString()
        {
            return this.FullName;
        }

    }
}
