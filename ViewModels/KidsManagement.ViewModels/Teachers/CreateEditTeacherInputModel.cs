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
        [MinLength(Constants.humanNameMinLen), MaxLength(Constants.humanNameMaxLen)]
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [Required]
        [MinLength(Constants.humanNameMinLen), MaxLength(Constants.humanNameMaxLen)]
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [Required]
        public Gender Gender { get; set; }
        [Required]
        public decimal Salary { get; set; }
        [Required]
        [DisplayName("Hiring Date")]
        public DateTime HiringDate { get; set; }
        
        [DisplayName("Dissmisal Date")]
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
