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
            Groups = new List<SingleGroupDetailsViewModel>();
        }

        public int StudentId { get; set; } //do I even need this?

        public List<SingleGroupDetailsViewModel> Groups {get;set;}
    }
}
