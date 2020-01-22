using KidsManagement.Data;
using KidsManagement.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kids.Management.Data.Models
{
    public class Admin
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(Const.humanNameMinLen),MaxLength(Const.humanNameMaxLen)]
        public string FirstName { get; set; }
        [Required]
        [MinLength(Const.humanNameMinLen), MaxLength(Const.humanNameMaxLen)]
        public string LastName { get; set; }
        
        [Required]
        public Gender Gender { get; set; }

        [Required]
        public DateTime HireDate { get; set; }

        public DateTime? DismissalDate { get; set; }


        [Required]
        public decimal Salary { get; set; }


        public IEnumerable<Note> AdminNotes { get; set; }

        //public DateTime/TimeSpan WorkingDays { get; set; }
    }
}
