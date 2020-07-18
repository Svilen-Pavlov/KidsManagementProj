﻿using KidsManagement.Data.Models;
using KidsManagement.Data.Models.Constants;
using KidsManagement.Data.Models.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KidsManagement.ViewModels.Parents
{
    public class CreateParentInputModel
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
        public string PhoneNumber { get; set; }
        public string AlternativePhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [EmailAddress]
        public string AlternativeEmail { get; set; }


        [Required]
        public Gender Gender { get; set; }

        [Required]
        public IFormFile ProfileImage { get; set; }

        public IEnumerable<Student> Children { get; set; }

        public string InitialAdminNote { get; set; }
    }
}
