using System;

namespace KidsManagement.ViewModels.Teachers.MyZoneModels.MySchedule
{
    public class DayOfWeekTimeSlot
    {
        public string GroupName { get; set; }

        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public TimeSpan Duration { get; set; }


        //optional

        public string KnownColorName { get; set; } //http://www.flounder.com/csharp_color_table.htm
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int StudentsCount { get; set; }


    }
}