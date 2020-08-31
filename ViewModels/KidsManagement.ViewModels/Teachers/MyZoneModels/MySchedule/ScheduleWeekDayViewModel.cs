using System;
using System.Collections.Generic;

namespace KidsManagement.ViewModels.Teachers.MyZoneModels.MySchedule
{
    public class ScheduleWeekDayViewModel
    {
        public ScheduleWeekDayViewModel()
        {
            this.TimeSlots = new List<DayOfWeekTimeSlot>();

        }
        public DayOfWeek DayOfWeek { get; set; }

        public List<DayOfWeekTimeSlot> TimeSlots { get; set; }

        public override string ToString()
        {
            return this.DayOfWeek.ToString();
        }
    }
}