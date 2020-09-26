using System;
using System.Collections.Generic;
using System.Text;

namespace KidsManagement.ViewModels.Parents
{
    public class AllParentsDetailsViewModel
    {
        public IEnumerable<AllSingleParentsViewModel> Parents { get; set; }
    }
}
