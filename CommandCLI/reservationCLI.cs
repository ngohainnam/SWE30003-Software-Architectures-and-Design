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

                int choice = Convert.ToInt32(Console.ReadLine());
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
            bool makingReservation = true;
            while (makingReservation)
            {
                bool isTableReserved = false;
                Console.WriteLine("\nWhat table would you like to reserve? ");
                int tableNumber = Convert.ToInt32(Console.ReadLine());
                tableNumber--;
                for (int i = 0; i < reservation.TableList.Length; i++)
                {
                    if (reservation.TableList[i].GetTableNo == tableNumber && reservation.TableList[i].Reserve == true)
                    {
                        isTableReserved = true;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nThis table is already reserved, choose another table.");
                        Console.ResetColor();
                    }
                }
                tableNumber++;
                // If selected table was reserved then return to beginning of Make Reservation to try again.
                if (isTableReserved == true)
                {
                    continue;
                }

                Console.WriteLine("Who is this reservation under? ");
                string ?customerName = Console.ReadLine();

                if (customerName == "")
                {
                    Console.WriteLine("This cannot be empty.");
                    return;
                }

                string customerName1 = customerName ?? "";
                reservation.ReserveTable(tableNumber, customerName1);
                makingReservation = false;
            }
        }

        public void CancelReservation()
        {
            Console.WriteLine("\nWho is the owner of the reservation that you would like to cancel? ");
            string customerName = Console.ReadLine() ?? ""; // Use null-coalescing operator to handle null (just to avoid warning message)

            if (customerName == "")
            {
                Console.WriteLine("This cannot be empty.");
                return;
            }

            reservation.CancelReservation(customerName);
        }
    }
}
