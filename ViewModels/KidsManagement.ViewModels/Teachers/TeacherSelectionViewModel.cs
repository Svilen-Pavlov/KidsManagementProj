﻿using System;
using System.Collections.Generic;
using System.Text;

namespace KidsManagement.ViewModels.Teachers
{
    public class TeacherSelectionViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool Selected { get; set; } //for checkbox forms specifically
    }
}
