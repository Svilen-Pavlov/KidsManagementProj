using KidsManagement.Data.Models.Constants;
using KidsManagement.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [MinLength(Constants.Constants.humanNameMinLen), MaxLength(Constants.Constants.humanNameMaxLen)]
        public string FirstName { get; set; }
        [Required]
        [MinLength(Constants.Constants.humanNameMinLen), MaxLength(Constants.Constants.humanNameMaxLen)]
        public string LastName { get; set; }

        [NotMapped]
        public string FullName => string.Format("{0} {1}", FirstName, LastName);


        [Required]
        public Gender Gender { get; set; }

        [Required]
        public DateTime HireDate { get; set; }

        public DateTime? DismissalDate { get; set; }

        [Required]
        public decimal Salary { get; set; }

        public string ProfilePicURI { get; set; }

        public virtual ICollection<Note> AdminNotes { get; set; }


        public virtual string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public override string ToString()
        {
            return this.FullName;
        }
    }
}
