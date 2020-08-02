using KidsManagement.Data.Models.Constants;
using KidsManagement.Data.Models.Enums;
using KidsManagement.ViewModels.Groups;
using KidsManagement.ViewModels.Levels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KidsManagement.ViewModels.Teachers
{
    public class CreateTeacherInputModel
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
        public string ConfirmPassword { get; set; }
    }
}
