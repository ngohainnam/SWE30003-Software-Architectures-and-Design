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
            bool ordering = true;
            while (ordering)
            {
                Console.WriteLine("\nPlease choose an action:");
                Console.WriteLine("1: View Menu");
                Console.WriteLine("2: Add Item to Order");
                Console.WriteLine("3: View Current Order");
                Console.WriteLine("4: Complete Order");
                Console.WriteLine("5: Exit");

                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Invalid input, please enter a number.");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        fMenu.PrintMenu();
                        break;
                    case 2:
                        AddItemToOrder();
                        break;
                    case 3:
                        fOrder.PrintOrder();
                        break;
                    case 4:
                        CompleteOrder();
                        ordering = false; // Exit ordering process
                        break;
                    case 5:
                        ordering = false; // Exit without completing
                        break;
                    default:
                        Console.WriteLine("Invalid option, try again.");
                        break;
                }
            }
        }
        private void AddItemToOrder()
        {
            Console.Write("I haven't created this one. \n");
        }


        private void CompleteOrder()
        {
            fOrder.PrintOrder();
            Console.WriteLine("Your order has been placed. Thank you!");
        }
    }
}
