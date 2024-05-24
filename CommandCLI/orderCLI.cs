using Group01RestaurantSystem.Transaction;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;

namespace Group01RestaurantSystem.CommandCLI
{
    internal class orderCLI : Command
    {
        private Menu fMenu; //Instance of Menu class
        private Order fOrder; //Instance of Order class

        //Constructor to initialize Menu and Order objects
        public orderCLI()
        {
            fMenu = new Menu();
            fOrder = new Order();
        }

        //Override the Execute method to handle the order process
        public override void Execute()
        {
            bool continueOrdering = true;
            while (continueOrdering)
            {
                Command.ClearScreen(); //Clear the console screen
                fMenu.PrintMenu(); //Print the menu
                Console.WriteLine();
                fOrder.PrintOrder(); //Print the current order
                Console.WriteLine();
                Console.WriteLine("\nPlease select an option:");
                Console.WriteLine("1: Add Item to Order");
                Console.WriteLine("2: Remove Item from Order");
                Console.WriteLine("3: Complete Order");
                Console.WriteLine("4: Exit");
                Console.WriteLine("Enter your option: ");

                //Read and validate user input
                int choice;
                while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 4)
                {
                    Command.ClearScreen(); //Clear the console screen
                    fMenu.PrintMenu(); //Print the menu
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nInvalid option. Please try again.");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("\nPlease select an option:");
                    Console.WriteLine("1: Add Item to Order");
                    Console.WriteLine("2: Remove Item from Order");
                    Console.WriteLine("3: Complete Order");
                    Console.WriteLine("4: Exit");
                    Console.WriteLine("Enter your option: ");
                }

                //Handle user choices
                switch (choice)
                {
                    case 1:
                        AddItemToOrder(); //Add an item to the order
                        fOrder.PrintOrder(); //Print the updated order
                        break;

                    case 2:
                        RemoveItem(); //Remove an item from the order
                        fOrder.PrintOrder(); //Print the updated order
                        break;

                    case 3:
                        Command.ClearScreen(); //Clear the console screen
                        fMenu.PrintMenu(); //Print the menu
                        Console.WriteLine();
                        fOrder.PrintOrder(); //Print the current order
                        Console.WriteLine();
                        bool isValid = MakePayment(); //Process payment
                        if (!isValid)
                        {
                            return;
                        }
                        Database.Instance.AddOrder(fOrder); //Add the order to the database
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Order Completed. Thank you!");
                        Console.ForegroundColor = ConsoleColor.White;
                        continueOrdering = false; //Exit the loop
                        break;

                    case 4:
                        continueOrdering = false; //Exit the loop
                        break;

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid option. Please try again.");
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                }
            }
        }

        //Method to handle payment process
        public bool MakePayment()
        {
            Console.WriteLine("\nSelect payment option:");
            Console.WriteLine("1: Card payment");
            Console.WriteLine("2: Cash payment");
            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    Console.WriteLine("Enter your bank account number: ");
                    string account = Console.ReadLine() ?? "";
                    Payment cardPayment = new CardTransaction(account, fOrder.GetTotal()); //Process card payment
                    bool isValid1 = cardPayment.ProcessPayment();
                    if (!isValid1)
                    {
                        return false;
                    }
                    return true;

                case 2:
                    Console.WriteLine("Enter the amount of cash you want to give: ");
                    int cash = Convert.ToInt32(Console.ReadLine());
                    Payment cashPayment = new CashTransaction(cash, fOrder.GetTotal()); //Process cash payment
                    bool isValid2 = cashPayment.ProcessPayment();
                    if (!isValid2)
                    {
                        return false;
                    }
                    return true;
            }
            return false;
        }

        //Method to add an item to the order
        private void AddItemToOrder()
        {
            int index;
            bool isValidInput = false;
            Command.ClearScreen(); //Clear the console screen
            fMenu.PrintMenu(); //Print the menu
            Console.WriteLine();
            fOrder.PrintOrder(); //Print the current order
            Console.WriteLine();

            while (!isValidInput)
            {
                Console.WriteLine("Enter the index of the item to add:");
                string input = Console.ReadLine() ?? "";

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Input cannot be empty. Please enter a valid index.");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    Command.ClearScreen(); //Clear the console screen
                    fMenu.PrintMenu(); //Print the menu
                    Console.WriteLine();
                    continue;
                }

                if (int.TryParse(input, out index))
                {
                    try
                    {
                        var item = fMenu.GetMenuItem(index - 1); //Adjust for zero-based index
                        fOrder.AddItem(item); //Add the item to the order
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Added {item.Name} to your order.");
                        Console.ForegroundColor = ConsoleColor.White;
                        isValidInput = true;
                    }
                    catch (IndexOutOfRangeException)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid index. Please try again.");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input. Please enter a numeric index.");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }

        //Method to remove an item from the order
        private void RemoveItem()
        {
            int index;
            bool isValidInput = false;
            Command.ClearScreen(); //Clear the console screen
            fMenu.PrintMenu(); //Print the menu
            Console.WriteLine();
            fOrder.PrintOrder(); //Print the current order
            Console.WriteLine();

            while (!isValidInput)
            {
                Console.WriteLine("Enter the index of the item (in the order) to remove:");
                string input = Console.ReadLine() ?? "";
                Command.ClearScreen(); //Clear the console screen
                fMenu.PrintMenu(); //Print the menu
                Console.WriteLine();
                fOrder.PrintOrder(); //Print the current order
                Console.WriteLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Input cannot be empty. Please enter a valid index.");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    Command.ClearScreen(); //Clear the console screen
                    fMenu.PrintMenu(); //Print the menu
                    Console.WriteLine();
                    continue;
                }

                if (int.TryParse(input, out index))
                {
                    index -= 1; //Adjust for zero-based index
                    try
                    {
                        var item = fOrder.GetOrderItem(index); //Get the item to be removed
                        fOrder.RemoveItem(item); //Remove the item from the order
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Removed {item.Name} from your order.");
                        Console.ForegroundColor = ConsoleColor.White;
                        isValidInput = true;
                    }
                    catch (IndexOutOfRangeException)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("There is no item like that in your order. Please try again.");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input. Please enter a numeric index.");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }
    }
}
