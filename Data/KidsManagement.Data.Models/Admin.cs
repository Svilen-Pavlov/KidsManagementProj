using KidsManagement.Data.Models.Constants;
using KidsManagement.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KidsManagement.Data.Models
{
    public class Admin
    {
        public Admin()
        {
            this.AdminNotes = new HashSet<Note>();
        }
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

        public string ProfilePicURI { get; set; }

        public virtual ICollection<Note> AdminNotes { get; set; }

    }
}
