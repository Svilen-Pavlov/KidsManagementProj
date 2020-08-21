using System;
using System.Collections.Generic;
using System.Text;

namespace KidsManagement.ViewModels.Teachers.MyZoneModels
{
    public class GroupStartsEndsSoonModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
