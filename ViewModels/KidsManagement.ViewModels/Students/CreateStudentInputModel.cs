using KidsManagement.Data;
using KidsManagement.Data.Models.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace KidsManagement.ViewModels.Students
{
    public class CreateStudentInputModel
    {

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
        public DateTime BirthDate { get; set; }

        [Required]
        public GradeLevel Grade { get; set; }

        [Required]
        public StudentStatus Status { get; set; }

        public int GroupId { get; set; }

        public IFormFile ProfileImage { get; set; }
    }
}
