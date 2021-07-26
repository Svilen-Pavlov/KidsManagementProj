using KidsManagement.Data.Models;
using KidsManagement.Data.Models.Enums;
using KidsManagement.ViewModels.Notes;
using KidsManagement.ViewModels.Students;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace KidsManagement.ViewModels.Parents
{
    public class ParentsDetailsViewModel
    {
        public int Id { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Middle Name")]
        public string MiddleName { get; set; }


        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Full Name")]
        public string FullName => string.Format("{0} {1}", FirstName, LastName);

        [DisplayName("Gender")]
        public Gender Gender { get; set; }

        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }
        [DisplayName("Alt. Phone Number")]
        public string AlternativePhoneNumber { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Alt. Email")]
        public string AlternativeEmail { get; set; }
        
        [DisplayName("Status")]
        public ParentStatus Status { get; set; }

        public string ProfilePicURI { get; set; }

        [DisplayName("Admin Notes")]
        public IEnumerable<NotesSelectionViewModel> AdminNotes { get; set; } 

        public IEnumerable<StudentSelectionViewModel> Children { get; set; } 
    }
}
