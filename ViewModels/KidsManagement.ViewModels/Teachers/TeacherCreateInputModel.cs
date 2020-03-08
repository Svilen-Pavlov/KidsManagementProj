using KidsManagement.Data;
using KidsManagement.Data.Models;
using KidsManagement.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace KidsManagement.ViewModels.Teachers
{
    public class TeacherCreateInputModel
    {
        [Required]
        [MinLength(Const.humanNameMinLen), MaxLength(Const.humanNameMaxLen)]
        public string FirstName { get; set; }
        [Required]
        [MinLength(Const.humanNameMinLen), MaxLength(Const.humanNameMaxLen)]
        public string LastName { get; set; }
        [Required]
        public Gender Gender { get; set; }
        [Required]
        public decimal Salary { get; set; }
        [Required]
        public DateTime HiringDate { get; set; }
        public DateTime? DismissalDate { get; set; }

        public IEnumerable<string> QualifiedLevels { get; set; }

      

    }
}
