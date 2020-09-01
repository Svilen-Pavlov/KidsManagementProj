using System;

namespace KidsManagement.ViewModels.Teachers.MyZoneModels.MySchedule
{
    public class DayOfWeekTimeSlot
    {
        public string GroupName { get; set; }

        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Duration { get; set; }


        //optional

        public string KnownColorName { get; set; } //http://www.flounder.com/csharp_color_table.htm
        public string StartDate { get; set; }
        public string EndDate { get; set; }

        public int StudentsCount { get; set; }


    }
}