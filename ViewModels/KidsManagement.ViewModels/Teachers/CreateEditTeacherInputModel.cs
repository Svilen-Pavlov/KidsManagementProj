using CustomExtensions.Attributes;
using KidsManagement.Data.Models.Constants;
using KidsManagement.Data.Models.Enums;
using KidsManagement.ViewModels.Groups;
using KidsManagement.ViewModels.Levels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KidsManagement.ViewModels.Teachers
{
    public class CreateEditTeacherInputModel
    {
        public int Id { get; set; } //for edit only

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
        [Range(1, double.MaxValue, ErrorMessage = Warnings.SalaryPositive)]
        public decimal Salary { get; set; }

        [Required]
        [DisplayName("Hiring Date")]
        public DateTime? HiringDate { get; set; }

        
       
        [Required]
        [Phone(ErrorMessage = Warnings.CreatePhone)]
        [DisplayName("Phone Number")]

        public string PhoneNumber { get; set; }
        [DateGreaterThanAttribute("HiringDate")]
        [DisplayName("Dissmisal Date (optional)")] 
        public DateTime? DismissalDate { get; set; }

        public string ProfilePicURI { get; set; } //for edit only

        [DisplayName("Profile Picture")]
        public IFormFile ProfileImage { get; set; }
        public List<LevelSelectionViewModel> Levels { get; set; } = new List<LevelSelectionViewModel>();
        public List<GroupSelectionViewModel> Groups { get; set; } = new List<GroupSelectionViewModel>();

        //ApplicationUser fields

        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
