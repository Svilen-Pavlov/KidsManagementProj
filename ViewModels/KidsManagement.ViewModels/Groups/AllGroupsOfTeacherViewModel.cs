﻿using System;
using System.Collections.Generic;
using System.Text;

namespace KidsManagement.ViewModels.Groups
{
    public class AllGroupsOfTeacherViewModel
    {
        public string TeacherName { get; set; }
        public IEnumerable<SingleGroupOfTeacherDetailsViewModel> Groups { get; set; }

    }
}
