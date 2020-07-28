using KidsManagement.Data.Models.Enums;
using KidsManagement.ViewModels.Students;
using System;
using System.Collections.Generic;
using System.Text;

namespace KidsManagement.ViewModels.Parents
{
    public class ParentsDetailsViewModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }


        public string LastName { get; set; }

        public string FullName => string.Format("{0} {1}", FirstName, LastName);

        public Gender Gender { get; set; }

        public string PhoneNumber { get; set; }
        public string AlternativePhoneNumber { get; set; }

        public string Email { get; set; }

        public string AlternativeEmail { get; set; }

        public string ProfilePicURI { get; set; }

        //public IEnumerable<Note> AdminNotes { get; set; } TODO

        public IEnumerable<StudentSelectionViewModel> Children { get; set; } 
    }
}
