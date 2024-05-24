using System;
using System.Collections.Generic;

namespace Group01RestaurantSystem
{
    //Class representing the reservation system for the restaurant
    public class Reservation
    {
        //Constructor to initialize the Reservation class
        public Reservation()
        {
        }

        //Method to list all tables and their reservation status
        public void ListAllTables()
        {
            Console.WriteLine("\nAll tables in the cafe:");
            foreach (Table table in Database.Instance.Tables)
            {
                int tableNo = table.TableNo + 1;
                Console.WriteLine("\nTable " + tableNo);

                int totalSlots = Table.TimeSlotConstants.DaysPerWeek * Table.TimeSlotConstants.HoursPerDay;
                int bookedSlots = 0;

                List<string> customerBookings = new List<string>();

                //Iterate through all time slots to check for bookings
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

                //Determine the booking status of the table
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

                //Display the names of customers who have booked the table
                if (customerBookings.Count > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Name of Customers: " + string.Join(", ", customerBookings));
                    Console.ResetColor();
                }
            }
        }

        //Property to get the list of tables from the database
        public List<Table> TableList => Database.Instance.Tables;

        //Method to get user input for the day of the week
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
                    Console.WriteLine("\n ~~~~~Invalid input. Please enter a valid number corresponding to a day of the week (1-7).~~~~~");
                    Console.ResetColor();
                }
            }
        }

        //Method to book a time slot for a table
        public bool BookTimeSlot(int tableNumber, Table.DayOfWeek day, int hour, string customerName)
        {
            tableNumber--;
            int dayIndex = (int)day;
            int hourIndex = hour - Table.TimeSlotConstants.StartHour;

            //Check if the provided day and hour are valid
            if (dayIndex < 0 || dayIndex >= Table.TimeSlotConstants.DaysPerWeek ||
                hourIndex < 0 || hourIndex >= Table.TimeSlotConstants.HoursPerDay)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n~~~~~Invalid hour!~~~~~");
                Console.ResetColor();
                return false;
            }

            //Check if the time slot is available and book it if it is
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
                    Database.Instance.SaveReservation(); //Save after booking
                    break;
                }
            }
            return true;
        }

        //Method to cancel a booking for a given customer name
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
                            Database.Instance.SaveReservation(); //Save after canceling
                            return;
                        }
                    }
                }
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n~~~~~No booking found for the given customer name.~~~~~");
            Console.ResetColor();
        }

        //Method to check if a time slot is available
        public bool IsTimeSlotAvailable(int tableNumber, Table.DayOfWeek day, int hour)
        {
            tableNumber--;
            int dayIndex = (int)day;
            int hourIndex = hour - Table.TimeSlotConstants.StartHour;

            //Check if the provided day and hour are valid
            if (dayIndex < 0 || dayIndex >= Table.TimeSlotConstants.DaysPerWeek ||
                hourIndex < 0 || hourIndex >= Table.TimeSlotConstants.HoursPerDay)
            {
                throw new ArgumentOutOfRangeException("\n~~~~~Invalid day or hour.~~~~~");
            }

            //Check if the time slot is booked
            foreach (var table in Database.Instance.Tables)
            {
                if (table.TableNo == tableNumber)
                {
                    if (table.TimeSlots[dayIndex][hourIndex]) //True means booked
                    {
                        return false; //False means it's not available
                    }
                    break;
                }
            }
            return true;
        }

        //Method to display available time slots for a table
        public void DisplayTimeSlots(int tableNumber)
        {
            tableNumber--;
            Console.WriteLine("\nBookings Time for " + (tableNumber + 1));
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
                            if (table.TimeSlots[day][hour - Table.TimeSlotConstants.StartHour])
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

        //Method to display user bookings for a table
        public void DisplayUserSlots(int tableNumber)
        {
            tableNumber--;
            Console.WriteLine("\nBookings for " + (tableNumber + 1));
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
