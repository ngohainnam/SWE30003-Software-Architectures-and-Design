using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group01RestaurantSystem.CommandCLI
{
    internal class reservationCLI : Command
    {
        private Reservation reservation; // Instance of Reservation class to manage reservations

        // Constructor to initialize the Reservation object
        public reservationCLI()
        {
            reservation = new Reservation();
        }

        // Override the Execute method to handle reservation-related actions
        public override void Execute()
        {
            bool continueReservation = true;
            while (continueReservation)
            {
                Console.Clear();
                ListAllTables(); // List all available tables
                Console.WriteLine("\nWhat would you like to do?");
                Console.WriteLine("1: Make Reservation");
                Console.WriteLine("2: Cancel Reservation");
                Console.WriteLine("3: Exit");
                Console.WriteLine("Enter your option: ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                String? input = Console.ReadLine();
                while (string.IsNullOrWhiteSpace(input))
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid option. Please try again.");
                    Console.ForegroundColor = ConsoleColor.White;
                    ListAllTables();
                    Console.WriteLine("\nWhat would you like to do?");
                    Console.WriteLine("1: Make Reservation");
                    Console.WriteLine("2: Cancel Reservation");
                    Console.WriteLine("3: Exit");
                    Console.WriteLine("Enter your option: ");
                    input = Console.ReadLine();
                }
                int choice = Convert.ToInt32(input);
                Console.ResetColor();

                // Handle user choices
                switch (choice)
                {
                    case 1:
                        MakeReservation(); // Make a reservation
                        break;

                    case 2:
                        CancelReservation(); // Cancel a reservation
                        break;

                    case 3:
                        continueReservation = false; // Exit the loop
                        break;
                }
            }
        }

        // Method to list all tables
        public void ListAllTables()
        {
            reservation.ListAllTables(); // Call the ListAllTables method from the Reservation class
        }

        // Method to make a reservation
        public void MakeReservation()
        {
            bool successfulBooking = false;
            var attempts = 0;
            string? customerName = "";

            while (!successfulBooking)
            {
                attempts++;
                if (attempts > 1)
                {
                    Console.WriteLine("\nTry a different table number, which table would you like to reserve?");
                }
                else
                {
                    Console.WriteLine("\nWhat table would you like to reserve?");
                }

                Console.ForegroundColor = ConsoleColor.Yellow;
                string? input = Console.ReadLine();
                while (string.IsNullOrWhiteSpace(input))
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid option. Please try again.");
                    Console.ForegroundColor = ConsoleColor.White;
                    ListAllTables();
                    Console.WriteLine("\nWhat table would you like to reserve?");
                    input = Console.ReadLine();
                }
                int tableNumber = Convert.ToInt32(input);
                Console.ResetColor();


                if (attempts == 1)
                {
                    Console.WriteLine("Who is this reservation under?");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    customerName = Console.ReadLine() ?? "";
                    Console.ResetColor();

                    if (customerName == "")
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n~~~~~This cannot be empty.~~~~~");
                        Console.ResetColor();
                        return;
                    }
                }

                Console.Clear();
                Console.WriteLine("Here are the booking time slots");
                reservation.DisplayTimeSlots(tableNumber); // Display available time slots for the table

                Table.DayOfWeek day = reservation.GetUserDayOfWeekInput(); // Get the day of the week from the user

                Console.WriteLine("\nWhat time slot? (Write the hour only)");
                Console.ForegroundColor = ConsoleColor.Yellow;
                int time = Convert.ToInt32(Console.ReadLine());
                Console.ResetColor();

                successfulBooking = reservation.BookTimeSlot(tableNumber, day, time, customerName); // Attempt to book the time slot

                if (successfulBooking)
                {
                    reservation.DisplayUserSlots(tableNumber); // Display the booked slots
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            }
        }

        // Method to cancel a reservation
        public void CancelReservation()
        {
            Console.WriteLine("\nWho is the owner of the reservation that you would like to cancel?");
            Console.ForegroundColor = ConsoleColor.Yellow;
            string? customerName = Console.ReadLine() ?? "";
            Console.ResetColor();

            if (customerName == "")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n~~~~~This cannot be empty.~~~~~");
                Console.ResetColor();
                return;
            }

            reservation.CancelTimeSlot(customerName); // Cancel the reservation for the customer
            Console.Clear();
        }
    }
}
