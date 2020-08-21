using System;
using System.Collections.Generic;
using System.Text;

namespace KidsManagement.ViewModels.Teachers.MyZoneModels
{
    public class TeacherMyZoneViewModel
    {
        public TeacherMyZoneViewModel()
        {
            this.Statistics = new MyZoneStatistics();
            this.Notifications = new HashSet<MyZoneNotifications>();
        }

        public MyZoneStatistics Statistics { get; set; }

        public IEnumerable<MyZoneNotifications> Notifications { get; set; }

    }
 
}
