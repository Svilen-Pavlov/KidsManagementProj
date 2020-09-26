using KidsManagement.Data.Models.Constants;
using KidsManagement.Data.Models.Enums;
using KidsManagement.ViewModels.Parents;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KidsManagement.ViewModels.Students
{
    public class CreateStudentInputModel
    {
        public CreateStudentInputModel()
        {
            Parents = new List<ParentsSelectionViewModel>();
        }

        public int Id { get; set; } //only for Edit

        [Required]
        [MinLength(Const.humanNameMinLen), MaxLength(Const.humanNameMaxLen)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(Const.humanNameMinLen), MaxLength(Const.humanNameMaxLen)]
        public string MiddleName { get; set; }

        [Required]
        [MinLength(Const.humanNameMinLen), MaxLength(Const.humanNameMaxLen)]
        public string LastName { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:MM}")]
        public DateTime BirthDate { get; set; }

        [Required]
        public GradeLevel Grade { get; set; }

        [Required]
        public StudentStatus Status { get; set; }

        public string ProfilePicURI { get; set; } //only for edit
        public IFormFile ProfileImage { get; set; }
        public int GroupId { get; set; }


        public List<ParentsSelectionViewModel> Parents { get; set; }
    }
}
