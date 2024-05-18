using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
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
                if (table.Reserve == false)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Status: Not Reserved");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Status: Reserved");
                    Console.WriteLine("Name of Customer: " + table.CustomerName);
                    Console.ResetColor();
                }
            }
        }
        public Table[] TableList
        {
            get { return tables; }
        }

        public void ReserveTable(int tableNumber, string customerName)
        {
            tableNumber--; //Reduce by 1 to match the numbering
            if (tableNumber < 0 || tableNumber > tables.Length)
            {
                Console.WriteLine("This table does not exist");
            }
            for (int i = 0; i < tables.Length; i++)
            {
                if (tables[i].GetTableNo == tableNumber && tables[i].Reserve == true)
                {
                    Console.WriteLine("You cannot make a reservation here, somebody else has already booked this table");
                }
                if (tables[i].GetTableNo == tableNumber && tables[i].Reserve == false)
                {
                    tables[i].Reserve = true;
                    tables[i].CustomerName = customerName;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nTable " + tables[i].GetTableNo + " is successfully reserved for " + tables[i].CustomerName);
                    Console.ResetColor();
                }
            }
        }

        public void CancelReservation(string customerName)
        {
            bool found = false;
            for (int i = 0; i < tables.Length; i++)
            {
                if (tables[i].CustomerName == customerName)
                {
                    found = true;
                    tables[i].Reserve = false;
                    tables[i].CustomerName = "";
                    Console.WriteLine("Reservation for " + tables[i].CustomerName + " has been cancelled!");
                    return;
                }
            }

            if (found == false)
            {
                Console.WriteLine(customerName + " not found in reservations.");
            }
        }
    }
}