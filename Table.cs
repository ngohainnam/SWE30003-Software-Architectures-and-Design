using System;

namespace Group01RestaurantSystem
{
    /// <summary>
    /// Represents a table in the restaurant.
    /// </summary>
    public class Table
    {
        /// <summary>
        /// Table number.
        /// </summary>
        public int TableNo { get; set; }

        /// <summary>
        /// Name of the customer who reserved the table.
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 2D array to represent the availability of time slots.
        /// </summary>
        public bool[][] TimeSlots { get; set; }

        /// <summary>
        /// 2D array to represent the user who reserved each time slot.
        /// </summary>
        public string[][] UserSlots { get; set; }

        /// <summary>
        /// Days of the week.
        /// </summary>
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

        /// <summary>
        /// Constants related to time slots.
        /// </summary>
        public static class TimeSlotConstants
        {
            public const int StartHour = 9;  // Start time (9 AM)
            public const int EndHour = 17;   // End time (5 PM)
            public const int HoursPerDay = EndHour - StartHour; // Total hours per day
            public const int DaysPerWeek = 7;  // Total days in a week
        }

        /// <summary>
        /// Initializes a new instance of the Table class with empty time slots.
        /// </summary>
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

        /// <summary>
        /// Initializes a new instance of the Table class with a specified table number.
        /// </summary>
        /// <param name="number">Table number.</param>
        public Table(int number) : this()
        {
            TableNo = number;
            CustomerName = "";
        }

        /// <summary>
        /// Converts a string to a DayOfWeek enum value.
        /// </summary>
        /// <param name="day">String representation of the day.</param>
        /// <returns>DayOfWeek value.</returns>
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
