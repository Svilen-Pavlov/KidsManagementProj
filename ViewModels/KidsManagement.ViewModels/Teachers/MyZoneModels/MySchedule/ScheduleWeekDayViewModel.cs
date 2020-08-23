using System;
using System.Collections.Generic;

namespace KidsManagement.ViewModels.Teachers.MyZoneModels.MySchedule
{
    public class ScheduleWeekDayViewModel
    {
        public DayOfWeek WeekDay { get; set; }

        public List<WeekDayTimeSlot> TimeSlots { get; set; }
    }
}