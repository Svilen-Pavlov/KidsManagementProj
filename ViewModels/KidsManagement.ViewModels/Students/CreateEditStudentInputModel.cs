using CustomExtensions.Attributes;
using KidsManagement.Data.Models.Constants;
using KidsManagement.Data.Models.Enums;
using KidsManagement.ViewModels.Parents;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KidsManagement.ViewModels.Students
{
    public class CreateEditStudentInputModel
    {
        public CreateEditStudentInputModel()
        {
            Parents = new List<ParentsSelectionViewModel>();
        }

        public int Id { get; set; } //only for Edit

        [Required]
        [DisplayName("First Name")]
        [RegularExpression(Constants.namesRegex, ErrorMessage = Warnings.CreatEntityName)]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("Middle Name")]
        [RegularExpression(Constants.namesRegex, ErrorMessage =Warnings.CreatEntityName)]
        public string MiddleName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        [RegularExpression(Constants.namesRegex, ErrorMessage = Warnings.CreatEntityName)]
        public string LastName { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = Warnings.RequiredBirthDate)]
        [DisplayName("Date of birth")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:MM}")]
        [DateIsInPast]
        public DateTime? BirthDate { get; set; }

        [Required]
        public GradeLevel Grade { get; set; }

        [Required]
        public StudentStatus Status { get; set; }

        public string ProfilePicURI { get; set; } //only for edit

        [DisplayName("Upload profile picture")]
        public IFormFile ProfileImage { get; set; }
        public int GroupId { get; set; }


        public List<ParentsSelectionViewModel> Parents { get; set; }
    }
}
