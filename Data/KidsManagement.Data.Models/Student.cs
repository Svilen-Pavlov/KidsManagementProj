using Kids.Management.Data.Models.Enums;
using KidsManagement.Data;
using KidsManagement.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Kids.Management.Data.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(Const.humanNameMinLen), MaxLength(Const.humanNameMaxLen)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(Const.humanNameMinLen), MaxLength(Const.humanNameMaxLen)]
        public string MiddleName { get; set; }

        [Required]
        [MinLength(Const.humanNameMinLen), MaxLength(Const.humanNameMaxLen)]
        public string LastName { get; set; }

        [NotMapped]
        public string FullName => string.Format("{0} {1} {2}", FirstName, MiddleName, LastName);

        [Required]
        public Gender Gender { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        public GradeLevel Grade { get; set; }




        [Required]
        public int GroupId { get; set; }
        public Group Group { get; set; }

        //many to many
        public IEnumerable<StudentParent> Parents { get; set; }

        //one to many
        public IEnumerable<Payment> Payments { get; set; }

        //many to many?? TODO
        public IEnumerable<Comment> TeacherComments { get; set; }



    }
}
