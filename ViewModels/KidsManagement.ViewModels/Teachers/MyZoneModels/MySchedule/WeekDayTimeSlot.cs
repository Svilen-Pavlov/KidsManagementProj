using System;

namespace KidsManagement.ViewModels.Teachers.MyZoneModels.MySchedule
{
    public class WeekDayTimeSlot
    {
        public string GroupName { get; set; }

        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public TimeSpan Duration { get; set; }


        //optional

        public string KnownColorName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int StudentsCount { get; set; }


    }
}