using System;

namespace Group01RestaurantSystem
{
    public class Table
    {
        public int TableNo { get; set; }
        public string CustomerName { get; set; }
        public bool[][] TimeSlots { get; set; }
        public string[][] UserSlots { get; set; }

        public enum DayOfWeek
        {
            Sunday,
            Monday,
            Tuesday,
            Wednesday,
            Thursday,
            Friday,
            Saturday
        }

        public static class TimeSlotConstants
        {
            public const int StartHour = 9;  // Start time (9 AM)
            public const int EndHour = 17;   // End time (5 PM)
            public const int HoursPerDay = EndHour - StartHour;
            public const int DaysPerWeek = 7;
        }

        public Table()
        {
            TimeSlots = new bool[TimeSlotConstants.DaysPerWeek][];
            UserSlots = new string[TimeSlotConstants.DaysPerWeek][];
            CustomerName = "";
            for (int day = 0; day < TimeSlotConstants.DaysPerWeek; day++)
            {
                TimeSlots[day] = new bool[TimeSlotConstants.HoursPerDay];
                UserSlots[day] = new string[TimeSlotConstants.HoursPerDay];
                for (int hour = 0; hour < TimeSlotConstants.HoursPerDay; hour++)
                {
                    UserSlots[day][hour] = "";
                }
            }
        }

        public Table(int number) : this()
        {
            TableNo = number;
            CustomerName = "";
        }

        public static DayOfWeek GetDayOfWeek(string day)
        {
            if (Enum.TryParse(day, true, out DayOfWeek dayOfWeek))
            {
                return dayOfWeek;
            }
            else
            {
                throw new ArgumentException("Invalid day of week");
            }
        }
    }
}
