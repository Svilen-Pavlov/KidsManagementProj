
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
    public class Parent
    {
        public Parent()
        {
            this.AdminNotes = new HashSet<Note>();
            this.Children = new HashSet<StudentParent>();
         
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

        public string ProfilePicURI { get; set; }

        public ParentStatus Status { get; set; }
        //many to many
        public virtual ICollection<Note> AdminNotes { get; set; }

        public virtual ICollection<StudentParent> Children { get; set; }

        public override string ToString()
        {
            return this.FullName;
        }
    }
}
