using System;
using System.Collections.Generic;
using System.Text;

namespace KidsManagement.ViewModels.Teachers
{
    public class AllTeachersListViewModel
    {
        public IEnumerable<TeachersListDetailsViewModel> Teachers { get; set; }
    }
}
