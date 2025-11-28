using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherMAUI.Models
{
    public class GroupedScheduleItems : List<CombinedScheduleItem>//class that handles Schedgule as a sorted list
    {
        public string DayOfWeek { get; private set; }//Day of Week for the timetable item

        public GroupedScheduleItems(string dayOfWeek, List<CombinedScheduleItem> items) : base(items)
        {
            DayOfWeek = dayOfWeek;
        }
    }
}
