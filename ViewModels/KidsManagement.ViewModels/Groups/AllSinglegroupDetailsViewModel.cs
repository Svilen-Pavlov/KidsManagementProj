﻿using KidsManagement.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace KidsManagement.ViewModels.Groups
{
    public class AllSinglegroupDetailsViewModel
    {
        [DisplayName("Id")]
        public int Id { get; set; }

        public string Name { get; set; }

        public string LevelName { get; set; }
        public DayOfWeek DayOfWeek { get; set; }

        public TimeSpan StartTime { get; set; }

        public string TeacherName { get; set; }

    }
}