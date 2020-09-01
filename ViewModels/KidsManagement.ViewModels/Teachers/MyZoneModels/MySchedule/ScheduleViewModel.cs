using System;
using System.Collections.Generic;
using System.Text;

namespace KidsManagement.ViewModels.Teachers.MyZoneModels.MySchedule
{
    public class ScheduleViewModel
    {
        public ScheduleViewModel()
        {
            this.ScheduleWeekDays = new List<ScheduleWeekDayViewModel>();
        }

        public string FromDateDisplay { get; set; }
        public string ToDateDisplay { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }


        //table formats

        //public int TableStartHour { get; set; } 8 
        //public int TableEndHour { get; set; } 20

        public List<ScheduleWeekDayViewModel> ScheduleWeekDays { get; set; }
    }
}
