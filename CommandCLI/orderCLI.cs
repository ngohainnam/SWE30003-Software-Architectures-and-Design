using System;
using System.Collections.Generic;

namespace Group01RestaurantSystem.CommandCLI
{
    internal class orderCLI : Command
    {
        private Menu fMenu;
        private Order fOrder;

        public orderCLI(List<string> commandList) : base(commandList)
        {
            fMenu = new Menu();
            fOrder = new Order();
        }

        public override void Execute()
        {
            bool continueOrdering = true;
            fMenu.PrintMenu();
            while (continueOrdering)
            {
                Console.WriteLine("\nPlease select an option:");
                Console.WriteLine("1: Add Item to Order");
                Console.WriteLine("2: Complete Order");
                Console.WriteLine("3: Exit");
                Console.WriteLine("Enter your option: ");

                int choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        AddItemToOrder();
                        break;
                    case 2:
                        fOrder.PrintOrder();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Order Completed. Thank you!");
                        Console.ForegroundColor = ConsoleColor.White;
                        continueOrdering = false;
                        break;
                    case 3:
                        continueOrdering = false;
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid option. Please try again.");
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                }
            }
        }

        private void AddItemToOrder()
        {
            Console.WriteLine("Enter the index of the item to add:");
            int index = Convert.ToInt32(Console.ReadLine()) - 1;
            try
            {
                var item = fMenu.GetMenuItem(index);
                fOrder.AddItem(item);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Added {item.GetName()} to your order.");
                Console.ForegroundColor = ConsoleColor.White;
            }
            catch (IndexOutOfRangeException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid index. Please try again.");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}
