using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Security.Cryptography.X509Certificates;

namespace Group01RestaurantSystem
{
    public class Table
    {
        private readonly int tableNo;
        // private bool reserved;
        private string customerName;
        private bool[,] timeSlots;

        private string[,] userSlot;

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

        public Table(int number)
        {
            this.timeSlots = new bool[TimeSlotConstants.DaysPerWeek, TimeSlotConstants.HoursPerDay];
            this.userSlot = new string[TimeSlotConstants.DaysPerWeek, TimeSlotConstants.HoursPerDay];
            for (int hour = Table.TimeSlotConstants.StartHour; hour < Table.TimeSlotConstants.EndHour; hour++)
                    {
                        for (int day = 0; day < Table.TimeSlotConstants.DaysPerWeek; day++)
                        {
                                userSlot[day, hour - Table.TimeSlotConstants.StartHour] = "";
                        }
                    }
            this.tableNo = number;
            this.customerName = "";
        }

        public int GetTableNo
        {
            get { return tableNo; }
        }

        public string CustomerName
        {
            get { return customerName; }
            set { customerName = value; }
        }

        public bool[,] TimeSlot
        {
            get { return timeSlots; }
            set { timeSlots = value; }
        }

        public string[,] UserSlot
        {
            get { return userSlot; }
            set { userSlot = value; }
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
