using KidsManagement.ViewModels.Groups;
using System;
using System.Collections.Generic;
using System.Text;

namespace KidsManagement.ViewModels.Students
{
    public class AddStudentToGroupInputModel
    {
        public AddStudentToGroupInputModel()
        {
            StudentIsInGroup = false;
            GroupsForSelection = new List<SingleGroupDetailsViewModel>();
        }
        public int IdIsSelected { get; set; } //specifically for selection

        public bool StudentIsInGroup { get; set; } 

        public List<SingleGroupDetailsViewModel> GroupsForSelection {get;set;}

    }
}
