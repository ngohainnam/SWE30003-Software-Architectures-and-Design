using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group01RestaurantSystem.CommandCLI
{
    internal class reservationCLI : Command
    {
        private Reservation reservation;
        public reservationCLI()
        {
            reservation = new Reservation();
        }

        public override void Execute()
        {
            bool continueReservation = true;
            while (continueReservation)
            {
                ListAllTables();
                Console.WriteLine("\nWhat would you like to do?");
                Console.WriteLine("1: Make Reservation");
                Console.WriteLine("2: Cancel Reservation");
                Console.WriteLine("3: Exit");
                Console.WriteLine("Enter your option: ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                int choice = Convert.ToInt32(Console.ReadLine());
                Console.ResetColor();
                switch (choice)
                {
                    case 1:
                        MakeReservation();
                        break;

                    case 2:
                        CancelReservation();
                        break;
                    case 3:
                        continueReservation = false;
                        break;
                }
            }
        }

        public void ListAllTables()
        {
            reservation.ListAllTables();
        }

        public void MakeReservation()
        {
            bool successfulBooking = false;
            var attempts = 0;
            string customerName = "";
            while (!successfulBooking)
            {
                attempts++;
                if (attempts > 1)
                {
                    Console.WriteLine("\nTry a different table number, which table would you like to reserve? ");
                }
                else
                {
                    Console.WriteLine("\nWhat table would you like to reserve? ");
                }
                Console.ForegroundColor = ConsoleColor.Yellow;
                int tableNumber = Convert.ToInt32(Console.ReadLine());
                Console.ResetColor();
                if (attempts == 1)
                {
                    Console.WriteLine("Who is this reservation under? ");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    customerName = Console.ReadLine();
                    Console.ResetColor();

                    if (customerName == "")
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n~~~~~This cannot be empty.~~~~~");
                        Console.ResetColor();
                        return;
                    }
                }

                Console.WriteLine("Here are the booking time slots");
                reservation.DisplayTimeSlots(tableNumber);

                DayOfWeek day = reservation.GetUserDayOfWeekInput();

                Console.WriteLine("\nWhat time slot? (Write the hour only)");
                Console.ForegroundColor = ConsoleColor.Yellow;
                int time = Convert.ToInt32(Console.ReadLine());
                Console.ResetColor();

                successfulBooking = reservation.BookTimeSlot(tableNumber, day, time, customerName);
                if (successfulBooking)
                {
                    reservation.DisplayUserSlots(tableNumber);
                }

            }
        }

        public void CancelReservation()
        {
            Console.WriteLine("\nWho is the owner of the reservation that you would like to cancel? ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            string customerName = Console.ReadLine();
            Console.ResetColor();

            if (customerName == "")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n~~~~~This cannot be empty.~~~~~");
                Console.ResetColor();
                return;
            }

            // reservation.CancelReservation(customerName);
            reservation.CancelTimeSlot(customerName);
        }
    }
}
