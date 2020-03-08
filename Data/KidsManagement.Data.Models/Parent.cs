
using KidsManagement.Data;
using KidsManagement.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace KidsManagement.Data.Models
{
    public class Parent
    {
        public Parent()
        {
            this.IsDeleted = false;
        }
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
        public string PhoneNumber { get; set; }
        public string AlternativePhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [EmailAddress]
        public string AlternativeEmail { get; set; }
                               

        //many to many
        public IEnumerable<Note> AdminNotes { get; set; }

        public IEnumerable<StudentParent> Children { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        public DateTime LastModified { get; set; }

        [Required]
        public bool IsDeleted { get; set; }
    }
}
