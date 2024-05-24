using System;

namespace Group01RestaurantSystem
{
    // Class representing a table in the restaurant
    public class Table
    {
        // Properties for the table number, customer name, time slots, and user slots
        public int TableNo { get; set; }               // Table number
        public string CustomerName { get; set; }       // Name of the customer who reserved the table
        public bool[][] TimeSlots { get; set; }        // 2D array to represent the availability of time slots
        public string[][] UserSlots { get; set; }      // 2D array to represent the user who reserved each time slot

        // Enum to represent days of the week
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

        // Static class to hold constants related to time slots
        public static class TimeSlotConstants
        {
            public const int StartHour = 9;  // Start time (9 AM)
            public const int EndHour = 17;   // End time (5 PM)
            public const int HoursPerDay = EndHour - StartHour; // Total hours per day
            public const int DaysPerWeek = 7;  // Total days in a week
        }

        // Default constructor to initialize the table with empty time slots
        public Table()
        {
            TimeSlots = new bool[TimeSlotConstants.DaysPerWeek][];
            UserSlots = new string[TimeSlotConstants.DaysPerWeek][];
            CustomerName = "";

            // Initialize each day with the appropriate number of hourly slots
            for (int day = 0; day < TimeSlotConstants.DaysPerWeek; day++)
            {
                TimeSlots[day] = new bool[TimeSlotConstants.HoursPerDay];
                UserSlots[day] = new string[TimeSlotConstants.HoursPerDay];
                for (int hour = 0; hour < TimeSlotConstants.HoursPerDay; hour++)
                {
                    UserSlots[day][hour] = ""; // Initialize user slots to empty strings
                }
            }
        }

        // Constructor to initialize the table with a specific number
        public Table(int number) : this()
        {
            TableNo = number;   // Set the table number
            CustomerName = "";  // Initialize the customer name to an empty string
        }

        // Static method to convert a string to a DayOfWeek enum value
        public static DayOfWeek GetDayOfWeek(string day)
        {
            if (Enum.TryParse(day, true, out DayOfWeek dayOfWeek))
            {
                return dayOfWeek; // Return the parsed day of the week
            }
            else
            {
                throw new ArgumentException("Invalid day of week"); // Throw an exception if parsing fails
            }
        }
    }
}
