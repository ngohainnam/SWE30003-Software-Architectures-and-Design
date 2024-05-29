using System;
using System.Collections.Generic;

namespace Group01RestaurantSystem
{
    /// <summary>
    /// Represents the reservation system for the restaurant.
    /// </summary>
    public class Reservation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Reservation"/> class.
        /// </summary>
        public Reservation()
        {
        }

        /// <summary>
        /// Lists all tables and their reservation status.
        /// </summary>
        public void ListAllTables()
        {
            Console.WriteLine("\nAll tables in the cafe:");
            foreach (Table table in Database.Instance.Tables)
            {
                int tableNo = table.TableNo + 1;
                Console.WriteLine($"\nTable {tableNo}");

                int totalSlots = Table.TimeSlotConstants.DaysPerWeek * Table.TimeSlotConstants.HoursPerDay;
                int bookedSlots = 0;
                List<string> customerBookings = new List<string>();

                for (int day = 0; day < Table.TimeSlotConstants.DaysPerWeek; day++)
                {
                    for (int hour = 0; hour < Table.TimeSlotConstants.HoursPerDay; hour++)
                    {
                        string customerName = table.UserSlots[day][hour];
                        if (!string.IsNullOrEmpty(customerName))
                        {
                            bookedSlots++;
                            string dayName = ((Table.DayOfWeek)day).ToString();
                            string timeOfBooking = $"{Table.TimeSlotConstants.StartHour + hour}:00";
                            customerBookings.Add($"{customerName} ({dayName}, {timeOfBooking})");
                        }
                    }
                }

                string status = bookedSlots == 0 ? "No Booking" : bookedSlots < totalSlots ? "Some Booking" : "Full";
                Console.ForegroundColor = bookedSlots == 0 ? ConsoleColor.Green : bookedSlots < totalSlots ? ConsoleColor.Yellow : ConsoleColor.Red;
                Console.WriteLine($"Status: {status}");
                Console.ResetColor();

                if (customerBookings.Count > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Name of Customers: {string.Join(", ", customerBookings)}");
                    Console.ResetColor();
                }
            }
        }

        /// <summary>
        /// Gets the list of tables from the database.
        /// </summary>
        public List<Table> TableList => Database.Instance.Tables;

        /// <summary>
        /// Gets user input for the day of the week.
        /// </summary>
        /// <returns>The day of the week as an enum.</returns>
        public Table.DayOfWeek GetUserDayOfWeekInput()
        {
            while (true)
            {
                Console.WriteLine("Please select a day of the week:");
                foreach (Table.DayOfWeek day in Enum.GetValues(typeof(Table.DayOfWeek)))
                {
                    Console.WriteLine($"{(int)day + 1}: {day}");
                }

                Console.Write("Enter the number corresponding to the day: ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                if (int.TryParse(Console.ReadLine(), out int dayIndex) && dayIndex >= 1 && dayIndex <= 7)
                {
                    Console.ResetColor();
                    return (Table.DayOfWeek)(dayIndex - 1);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n~~~~~Invalid input. Please enter a valid number corresponding to a day of the week (1-7).~~~~~");
                    Console.ResetColor();
                }
            }
        }

        /// <summary>
        /// Books a time slot for a table.
        /// </summary>
        /// <param name="tableNumber">Table number.</param>
        /// <param name="day">Day of the week.</param>
        /// <param name="hour">Hour of the day.</param>
        /// <param name="customerName">Customer's name.</param>
        /// <returns>True if booking is successful, otherwise false.</returns>
        public bool BookTimeSlot(int tableNumber, Table.DayOfWeek day, int hour, string customerName)
        {
            tableNumber--;
            int dayIndex = (int)day;
            int hourIndex = hour - Table.TimeSlotConstants.StartHour;

            if (dayIndex < 0 || dayIndex >= Table.TimeSlotConstants.DaysPerWeek ||
                hourIndex < 0 || hourIndex >= Table.TimeSlotConstants.HoursPerDay)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n~~~~~Invalid hour!~~~~~");
                Console.ResetColor();
                return false;
            }

            foreach (var table in Database.Instance.Tables)
            {
                if (table.TableNo == tableNumber)
                {
                    if (table.TimeSlots[dayIndex][hourIndex])
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n~~~~~This time slot is already booked!~~~~~");
                        Console.ResetColor();
                        return false;
                    }
                    table.TimeSlots[dayIndex][hourIndex] = true;
                    table.UserSlots[dayIndex][hourIndex] = customerName;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\n~~~~~Successful booking!~~~~~");
                    Console.ResetColor();
                    Database.Instance.SaveReservation(); // Save after booking
                    break;
                }
            }
            return true;
        }

        /// <summary>
        /// Cancels a booking for a given customer name.
        /// </summary>
        /// <param name="customerName">Customer's name.</param>
        public void CancelTimeSlot(string customerName)
        {
            foreach (var table in Database.Instance.Tables)
            {
                for (int day = 0; day < Table.TimeSlotConstants.DaysPerWeek; day++)
                {
                    for (int hour = 0; hour < Table.TimeSlotConstants.HoursPerDay; hour++)
                    {
                        if (table.UserSlots[day][hour] == customerName)
                        {
                            table.TimeSlots[day][hour] = false;
                            table.UserSlots[day][hour] = string.Empty;
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("\n~~~~~Booking canceled successfully!~~~~~");
                            Console.ResetColor();
                            Database.Instance.SaveReservation(); // Save after canceling
                            return;
                        }
                    }
                }
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n~~~~~No booking found for the given customer name.~~~~~");
            Console.ResetColor();
        }

        /// <summary>
        /// Checks if a time slot is available.
        /// </summary>
        /// <param name="tableNumber">Table number.</param>
        /// <param name="day">Day of the week.</param>
        /// <param name="hour">Hour of the day.</param>
        /// <returns>True if the slot is available, otherwise false.</returns>
        public bool IsTimeSlotAvailable(int tableNumber, Table.DayOfWeek day, int hour)
        {
            tableNumber--;
            int dayIndex = (int)day;
            int hourIndex = hour - Table.TimeSlotConstants.StartHour;

            if (dayIndex < 0 || dayIndex >= Table.TimeSlotConstants.DaysPerWeek ||
                hourIndex < 0 || hourIndex >= Table.TimeSlotConstants.HoursPerDay)
            {
                throw new ArgumentOutOfRangeException("Invalid day or hour.");
            }

            foreach (var table in Database.Instance.Tables)
            {
                if (table.TableNo == tableNumber && table.TimeSlots[dayIndex][hourIndex])
                {
                    return false; // The time slot is already booked.
                }
            }
            return true;
        }

        /// <summary>
        /// Displays available time slots for a specific table.
        /// </summary>
        /// <param name="tableNumber">Table number to display slots for.</param>
        public void DisplayTimeSlots(int tableNumber)
        {
            tableNumber--;
            Console.WriteLine($"\nBooking Times for Table {tableNumber + 1}");
            Console.WriteLine("   Hour | Sunday           | Monday           | Tuesday          | Wednesday        | Thursday         | Friday           | Saturday         |");
            Console.WriteLine("--------+------------------+------------------+------------------+------------------+------------------+------------------+------------------+");

            foreach (var table in Database.Instance.Tables)
            {
                if (table.TableNo == tableNumber)
                {
                    for (int hour = Table.TimeSlotConstants.StartHour; hour < Table.TimeSlotConstants.EndHour; hour++)
                    {
                        Console.Write($"{hour,2}:00   |");
                        for (int day = 0; day < Table.TimeSlotConstants.DaysPerWeek; day++)
                        {
                            Console.ForegroundColor = table.TimeSlots[day][hour - Table.TimeSlotConstants.StartHour] ? ConsoleColor.Red : ConsoleColor.Green;
                            Console.Write($"  {(table.TimeSlots[day][hour - Table.TimeSlotConstants.StartHour] ? "Unavailable" : "Available"),-15}");
                            Console.ResetColor();
                            Console.Write(" |");
                        }
                        Console.WriteLine();
                    }
                }
            }
        }

        /// <summary>
        /// Displays user bookings for a specific table.
        /// </summary>
        /// <param name="tableNumber">Table number to display bookings for.</param>
        public void DisplayUserSlots(int tableNumber)
        {
            tableNumber--;
            Console.WriteLine($"\nUser Bookings for Table {tableNumber + 1}");
            Console.WriteLine("   Hour | Sunday           | Monday           | Tuesday          | Wednesday        | Thursday         | Friday           | Saturday         |");
            Console.WriteLine("--------+------------------+------------------+------------------+------------------+------------------+------------------+------------------+");

            foreach (var table in Database.Instance.Tables)
            {
                if (table.TableNo == tableNumber)
                {
                    for (int hour = Table.TimeSlotConstants.StartHour; hour < Table.TimeSlotConstants.EndHour; hour++)
                    {
                        Console.Write($"{hour,2}:00   |");
                        for (int day = 0; day < Table.TimeSlotConstants.DaysPerWeek; day++)
                        {
                            string userSlot = table.UserSlots[day][hour - Table.TimeSlotConstants.StartHour];
                            Console.ForegroundColor = string.IsNullOrEmpty(userSlot) ? ConsoleColor.Green : ConsoleColor.Red;
                            Console.Write($" {(string.IsNullOrEmpty(userSlot) ? "Available" : userSlot),-16}");
                            Console.ResetColor();
                            Console.Write(" |");
                        }
                        Console.WriteLine();
                    }
                }
            }
        }
    }
}
