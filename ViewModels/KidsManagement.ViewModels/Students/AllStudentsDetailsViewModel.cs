using System;
using System.Collections.Generic;
using System.Text;

namespace KidsManagement.ViewModels.Students
{
    public class AllStudentsDetailsViewModel
    {
        public IEnumerable<AllSingleStudentsViewModel> Students { get; set; }
    }
}
