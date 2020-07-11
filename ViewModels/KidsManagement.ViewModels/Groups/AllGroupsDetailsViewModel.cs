using System;
using System.Collections.Generic;
using System.Text;

namespace KidsManagement.ViewModels.Groups
{
    public class AllGroupsDetailsViewModel
    {
        
        public IEnumerable<SingleGroupDetailsViewModel> Groups { get; set; }
    }
}
