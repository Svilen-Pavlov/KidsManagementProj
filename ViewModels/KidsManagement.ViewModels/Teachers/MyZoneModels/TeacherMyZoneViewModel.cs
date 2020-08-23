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
            this.Notifications = new HashSet<MyZoneNotification>();
        }

        public MyZoneStatistics Statistics { get; set; }

        public IEnumerable<MyZoneNotification> Notifications { get; set; }

    }
 
}
