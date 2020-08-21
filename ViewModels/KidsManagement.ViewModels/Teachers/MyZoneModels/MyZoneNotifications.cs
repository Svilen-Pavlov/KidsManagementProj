using System;
using System.Collections.Generic;
using System.Text;

namespace KidsManagement.ViewModels.Teachers.MyZoneModels
{
    public class MyZoneNotifications
    {
        public List<StudentBirthdaySoonModel> BirthdaysThisWeek { get; set; }
        public List<StudentBirthdaySoonModel> BirthdaysNextWeek { get; set; }

        public List<GroupStartsEndsSoonModel> GroupsStartingSoon { get; set; }
        public List<GroupStartsEndsSoonModel> GroupsEndingSoon { get; set; }
    }
}
