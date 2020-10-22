using KidsManagement.Data.Models;
using KidsManagement.Data.Models.Constants;
using KidsManagement.Data.Models.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KidsManagement.ViewModels.Parents
{
    public class CreateEditParentInputModel
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("First Name")]
        [RegularExpression(Constants.humanNamesRegex, ErrorMessage = Warnings.CreatHumanName)]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        [RegularExpression(Constants.humanNamesRegex, ErrorMessage = Warnings.CreatHumanName)]
        public string LastName { get; set; }


        [Required]
        public Gender Gender { get; set; }


        [Required]
        [Phone(ErrorMessage =Warnings.CreatePhone)]
        public string PhoneNumber { get; set; }

        [Phone(ErrorMessage = Warnings.CreatePhone)]
        public string AlternativePhoneNumber { get; set; }

        [Required]
        [EmailAddress(ErrorMessage =Warnings.CreateEmail)]
        public string Email { get; set; }

        [EmailAddress]
        public string AlternativeEmail { get; set; }



        public string ProfilePicURI { get; set; } //only for edit

        [Required]
        public IFormFile ProfileImage { get; set; }

        public IEnumerable<Student> Children { get; set; }

        public string InitialAdminNote { get; set; }
    }
}
