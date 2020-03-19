using System;
using System.Collections.Generic;
using System.Text;

namespace KidsManagement.ViewModels.Students
{
    public class AllStudentsDetailsViewModel
    {
        IEnumerable<AllSingleStudentsViewModel> Students { get; set; }
    }
}
