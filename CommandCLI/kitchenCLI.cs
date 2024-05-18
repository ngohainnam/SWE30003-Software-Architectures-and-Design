using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;

namespace Group01RestaurantSystem.CommandCLI
{
    internal class kitchenCLI : Command
    {
        public kitchenCLI()
        {

        }

        public override void Execute()
        {
            Console.Clear();
            while (true)
            {
                Console.WriteLine("1. View food queue");
                Console.WriteLine("2. Exit");
                Console.Write("Select an option: ");

                int choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        Console.WriteLine("Haven't done this yet");
                        break;
                    case 2:
                        return;

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid option. Please try again.");
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                }
            }
        }
    }
}