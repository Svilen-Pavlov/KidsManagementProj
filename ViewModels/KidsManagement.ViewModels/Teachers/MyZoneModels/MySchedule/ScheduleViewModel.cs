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

        public string FromDate { get; set; }
        public string ToDate { get; set; }


        //table formats

        //public int TableStartHour { get; set; } 8 
        //public int TableEndHour { get; set; } 20

        public List<ScheduleWeekDayViewModel> ScheduleWeekDays { get; set; }
    }
}
