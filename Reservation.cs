using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace Group01RestaurantSystem
{
    public class Reservation
    {
        //Initialize tables in the restaurant
        Table[] tables = new Table[10];
        public Reservation()
        {
            for (int i = 0; i < tables.Length; i++)
            {
                tables[i] = new Table(i);
            }
        }

        public void ListAllTables()
        {
            Console.WriteLine("\nAll tables in the cafe:");
            foreach (Table table in tables)
            {
                int tableNo = table.GetTableNo + 1;
                Console.WriteLine("\nTable " + tableNo);

                int totalSlots = Table.TimeSlotConstants.DaysPerWeek * Table.TimeSlotConstants.HoursPerDay;
                int bookedSlots = 0;

                List<string> customerBookings = new List<string>();

                for (int day = 0; day < Table.TimeSlotConstants.DaysPerWeek; day++)
                {
                    for (int hour = 0; hour < Table.TimeSlotConstants.HoursPerDay; hour++)
                    {
                        string customerName = table.UserSlot[day, hour];
                        if (!string.IsNullOrEmpty(customerName))
                        {
                            bookedSlots++;
                            string dayName = ((DayOfWeek)day).ToString();
                            string timeOfBooking = $"{Table.TimeSlotConstants.StartHour + hour}:00";
                            customerBookings.Add($"{customerName} ({dayName}, {timeOfBooking})");
                        }
                    }
                }

                string status;
                if (bookedSlots == 0)
                {
                    status = "No Booking";
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else if (bookedSlots < totalSlots)
                {
                    status = "Some Booking";
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                else
                {
                    status = "Full";
                    Console.ForegroundColor = ConsoleColor.Red;
                }

                Console.WriteLine("Status: " + status);
                Console.ResetColor();

                if (customerBookings.Count > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Name of Customers: " + string.Join(", ", customerBookings));
                    Console.ResetColor();
                }
            }
        }

        public Table[] TableList
        {
            get { return tables; }
        }

        public DayOfWeek GetUserDayOfWeekInput()
        {
            while (true)
            {
                Console.WriteLine("Please select a day of the week:");
                foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
                {
                    Console.WriteLine($"{(int)day + 1}: {day}");
                }
                Console.Write("Enter the number corresponding to the day: ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                if (int.TryParse(Console.ReadLine(), out int dayIndex) && dayIndex >= 1 && dayIndex <= 7)
                {
                    Console.ResetColor();
                    return (DayOfWeek)(dayIndex - 1);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n ~~~~~Invalid input. Please enter a valid number corresponding to a day of the week (1-7).~~~~~");
                    Console.ResetColor();
                }
            }
        }

        public bool BookTimeSlot(int tableNumber, DayOfWeek day, int hour, string customerName)
        {
            tableNumber--;
            int dayIndex = (int)day;
            int hourIndex = hour - Table.TimeSlotConstants.StartHour;

            if (dayIndex < 0 || dayIndex >= Table.TimeSlotConstants.DaysPerWeek + 1 ||
                hourIndex < 0 || hourIndex >= Table.TimeSlotConstants.HoursPerDay)
            {
                throw new ArgumentOutOfRangeException("\n~~~~~Invalid day or hour.~~~~~");

            }

            for (int i = 0; i < tables.Length; i++)
            {
                if (tables[i].GetTableNo == tableNumber)
                {
                    if (tables[i].TimeSlot[dayIndex, hourIndex])
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n~~~~~This time slot is already booked!~~~~~");
                        Console.ResetColor();
                        return false;
                    }
                    tables[i].TimeSlot[dayIndex, hourIndex] = true;
                    tables[i].UserSlot[dayIndex, hourIndex] = customerName;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\n~~~~~Successful booking!~~~~~");
                    Console.ResetColor();
                    break;
                }
            }
            return true;
        }

        public void CancelTimeSlot(string customerName)
        {
            for (int i = 0; i < tables.Length; i++)
            {
                for (int day = 0; day < Table.TimeSlotConstants.DaysPerWeek; day++)
                {
                    for (int hour = 0; hour < Table.TimeSlotConstants.HoursPerDay; hour++)
                    {
                        if (tables[i].UserSlot[day, hour] == customerName)
                        {
                            tables[i].TimeSlot[day, hour] = false;
                            tables[i].UserSlot[day, hour] = string.Empty;
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("\n~~~~~Booking canceled successfully!~~~~~");
                            Console.ResetColor();
                            return;
                        }
                    }
                }
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n~~~~~No booking found for the given customer name.~~~~~");
            Console.ResetColor();
        }

        public bool IsTimeSlotAvailable(int tableNumber, DayOfWeek day, int hour)
        {
            tableNumber--;
            int dayIndex = (int)day;
            int hourIndex = hour - Table.TimeSlotConstants.StartHour;

            if (dayIndex < 0 || dayIndex >= Table.TimeSlotConstants.DaysPerWeek ||
                hourIndex < 0 || hourIndex >= Table.TimeSlotConstants.HoursPerDay)
            {
                throw new ArgumentOutOfRangeException("\n~~~~~Invalid day or hour.~~~~~");
            }

            for (int i = 0; i < tables.Length; i++)
            {
                if (tables[i].GetTableNo == tableNumber)
                {
                    if (tables[i].TimeSlot[dayIndex, hourIndex] == true) // True means booked
                    {
                        return false; // False means its not available
                    }
                    break;
                }
            }
            return true;
        }
        public void DisplayTimeSlots(int tableNumber)
        {
            tableNumber--;
            Console.WriteLine("\nBookings Time for " + (tableNumber + 1));
            Console.WriteLine("   Hour | Sunday           | Monday           | Tuesday          | Wednesday        | Thursday         | Friday           | Saturday         |");
            Console.WriteLine("--------+------------------+------------------+------------------+------------------+------------------+------------------+------------------+");

            for (int i = 0; i < tables.Length; i++)
            {
                if (tables[i].GetTableNo == tableNumber)
                {
                    for (int hour = Table.TimeSlotConstants.StartHour; hour < Table.TimeSlotConstants.EndHour; hour++)
                    {
                        Console.Write($"{hour,2}:00   |");
                        for (int day = 0; day < Table.TimeSlotConstants.DaysPerWeek; day++)
                        {
                            if (tables[i].TimeSlot[day, hour - Table.TimeSlotConstants.StartHour])
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write("  Unavailable    ");
                                Console.ResetColor();
                                Console.Write(" |");
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Write("  Available      ");
                                Console.ResetColor();
                                Console.Write(" |");
                            }
                        }
                        Console.WriteLine();
                    }
                }
            }
        }
        public void DisplayUserSlots(int tableNumber)
        {
            tableNumber--;
            Console.WriteLine("\nBookings for " + (tableNumber + 1));
            Console.WriteLine("   Hour | Sunday           | Monday           | Tuesday          | Wednesday        | Thursday         | Friday           | Saturday         |");
            Console.WriteLine("--------+------------------+------------------+------------------+------------------+------------------+------------------+------------------+");

            for (int i = 0; i < tables.Length; i++)
            {
                if (tables[i].GetTableNo == tableNumber)
                {
                    for (int hour = Table.TimeSlotConstants.StartHour; hour < Table.TimeSlotConstants.EndHour; hour++)
                    {
                        Console.Write($"{hour,2}:00   |");
                        for (int day = 0; day < Table.TimeSlotConstants.DaysPerWeek; day++)
                        {
                            string userSlot = tables[i].UserSlot[day, hour - Table.TimeSlotConstants.StartHour];
                            if (!string.IsNullOrEmpty(userSlot))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write($" {userSlot,-16}");
                                Console.ResetColor();
                                Console.Write(" |");
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Write("  Available      ");
                                Console.ResetColor();
                                Console.Write(" |");
                            }
                        }
                        Console.WriteLine();
                    }
                }
            }
        }
    }
}