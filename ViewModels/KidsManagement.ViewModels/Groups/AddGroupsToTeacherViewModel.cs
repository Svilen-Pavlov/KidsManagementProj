using System;
using System.Collections.Generic;
using System.Text;

namespace KidsManagement.ViewModels.Groups
{
    public class AddGroupsToTeacherViewModel
    {
        public AddGroupsToTeacherViewModel()
        {
            this.Groups = new List<GroupSelectionViewModel>();
        }
        public int TeacherId { get; set; }

        public List<GroupSelectionViewModel> Groups {get;set;}
    }
}
