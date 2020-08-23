using System;
using System.Collections.Generic;
using System.Text;

namespace KidsManagement.ViewModels.Teachers.MyZoneModels.MySchedule
{
    public class ScheduleViewModel
    {
        public ScheduleViewModel()
        {
            this.ScheduleWeekDays = new HashSet<ScheduleWeekDayViewModel>();
        }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }


        public IEnumerable<ScheduleWeekDayViewModel> ScheduleWeekDays { get; set; }
    }
}
