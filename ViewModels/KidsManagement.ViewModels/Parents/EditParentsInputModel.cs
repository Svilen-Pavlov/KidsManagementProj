using System;
using System.Collections.Generic;
using System.Text;

namespace KidsManagement.ViewModels.Parents
{
    public class EditParentsInputModel
    {
        public EditParentsInputModel()
        {
            Parents = new List<ParentsSelectionViewModel>();
        }

        public int StudentId { get; set; }

        public List<ParentsSelectionViewModel> Parents { get; set; }
    }
}
