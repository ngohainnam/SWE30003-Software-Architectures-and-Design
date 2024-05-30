using Group01RestaurantSystem.Transaction;
using System;
using System.Collections.Generic;

namespace Group01RestaurantSystem.CommandCLI
{
    /// <summary>
    /// Class for handling order-related commands in the CLI.
    /// </summary>
    internal class OrderCLI : Command
    {
        private Menu fMenu;
        private Order fOrder;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderCLI"/> class.
        /// </summary>
        public OrderCLI()
        {
            fMenu = new Menu();
            fOrder = new Order();
        }

        /// <summary>
        /// Executes the order-related commands.
        /// </summary>
        public override void Execute()
        {
            bool continueOrdering = true;
            while (continueOrdering)
            {
                Console.Clear();
                fMenu.PrintMenu();
                Console.WriteLine();
                fOrder.PrintOrder();
                Console.WriteLine();
                Console.WriteLine("\nPlease select an option:");
                Console.WriteLine("1: Add Item to Order");
                Console.WriteLine("2: Remove Item from Order");
                Console.WriteLine("3: Complete Order");
                Console.WriteLine("4: Exit");
                Console.Write("Enter your option: ");

                // Read and validate user input
                int choice;
                while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 4)
                {
                    Console.Clear();
                    fMenu.PrintMenu();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nInvalid option. Please try again.");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("\nPlease select an option:");
                    Console.WriteLine("1: Add Item to Order");
                    Console.WriteLine("2: Remove Item from Order");
                    Console.WriteLine("3: Complete Order");
                    Console.WriteLine("4: Exit");
                    Console.Write("Enter your option: ");
                }

                switch (choice)
                {
                    case 1:
                        AddItemToOrder();
                        fOrder.PrintOrder(); // Print the updated order
                        break;

                    case 2:
                        RemoveItem();
                        fOrder.PrintOrder(); // Print the updated order
                        break;

                    case 3:
                        Console.Clear();
                        fMenu.PrintMenu();
                        Console.WriteLine();
                        fOrder.PrintReceipt();
                        Console.WriteLine();
                        bool isValid = MakePayment();
                        if (!isValid)
                        {
                            return;
                        }
                        Database.Instance.AddOrder(fOrder);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Order Completed. Thank you!");
                        Console.ForegroundColor = ConsoleColor.White;
                        continueOrdering = false; // Exit the loop
                        break;

                    case 4:
                        continueOrdering = false; // Exit the loop
                        break;

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid option. Please try again.");
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                }
            }
        }

        /// <summary>
        /// Handles the payment process.
        /// </summary>
        /// <returns>True if the payment is successful, otherwise false.</returns>
        public bool MakePayment()
        {
            Console.WriteLine("\nSelect payment option:");
            Console.WriteLine("1: Card payment");
            Console.WriteLine("2: Cash payment");
            string? input = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(input))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid option. Please try again.");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("1: Card payment");
                Console.WriteLine("2: Cash payment");
                input = Console.ReadLine();
            }
            int choice = Convert.ToInt32(input);

            switch (choice)
            {
                case 1:
                    Console.Write("Enter your bank account number: ");
                    string? account = Console.ReadLine();
                    while (string.IsNullOrWhiteSpace(account))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Account number cannot be empty. Please enter a valid account number.");
                        Console.ForegroundColor = ConsoleColor.White;
                        account = Console.ReadLine();
                    }
                    Payment cardPayment = new CardTransaction(account, fOrder.GetTotal()); // Process card payment
                    return cardPayment.ProcessPayment();

                case 2:
                    Console.Write("Enter the amount of cash you want to give: ");
                    int cash = Convert.ToInt32(Console.ReadLine());
                    Payment cashPayment = new CashTransaction(cash, fOrder.GetTotal()); // Process cash payment
                    return cashPayment.ProcessPayment();

                default:
                    return false;
            }
        }

        /// <summary>
        /// Adds an item to the order.
        /// </summary>
        private void AddItemToOrder()
        {
            int index;
            bool isValidInput = false;
            Console.Clear();
            fMenu.PrintMenu();
            Console.WriteLine();
            fOrder.PrintOrder();
            Console.WriteLine();

            while (!isValidInput)
            {
                Console.Write("Enter the index of the item to add: ");
                string input = Console.ReadLine() ?? "";

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Input cannot be empty. Please enter a valid index.");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                    fMenu.PrintMenu();
                    Console.WriteLine();
                    continue;
                }

                if (int.TryParse(input, out index))
                {
                    try
                    {
                        var item = fMenu.GetMenuItem(index - 1); // Adjust for zero-based index
                        fOrder.AddItem(item);
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

        /// <summary>
        /// Removes an item from the order.
        /// </summary>
        private void RemoveItem()
        {
            int index;
            bool isValidInput = false;
            Console.Clear();
            fMenu.PrintMenu();
            Console.WriteLine();
            fOrder.PrintOrder();
            Console.WriteLine();

            while (!isValidInput)
            {
                Console.Write("Enter the index of the item (in the order) to remove: ");
                string input = Console.ReadLine() ?? "";
                Console.Clear();
                fMenu.PrintMenu();
                Console.WriteLine();
                fOrder.PrintOrder();
                Console.WriteLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Input cannot be empty. Please enter a valid index.");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                    fMenu.PrintMenu();
                    Console.WriteLine();
                    continue;
                }

                if (int.TryParse(input, out index))
                {
                    index -= 1; // Adjust for zero-based index
                    try
                    {
                        var item = fOrder.GetOrderItem(index); // Get the item to be removed
                        fOrder.RemoveItem(item);
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
