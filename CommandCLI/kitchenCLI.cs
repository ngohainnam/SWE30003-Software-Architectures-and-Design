using System;
using System.Collections.Generic;

namespace Group01RestaurantSystem.CommandCLI
{
    //Class to handle kitchen command line interface operations, inheriting from the abstract Command class
    internal class kitchenCLI : Command
    {
        private int choice;
        //Constructor for kitchenCLI class
        public kitchenCLI()
        {
        }

        //Overridden Execute method to display and handle the kitchen menu
        public override void Execute()
        {
            Console.Clear();
            //Infinite loop to continuously display the menu until the user chooses to exit
            while (true)
            {
                //Display menu options
                Console.WriteLine("1. View food queue");
                Console.WriteLine("2. Exit");
                Console.Write("Select an option: ");

                //Read user input and convert it to an integer
                string? input = Console.ReadLine();
                while(string.IsNullOrWhiteSpace(input))
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid option. Please try again.");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("1. View food queue");
                    Console.WriteLine("2. Exit");
                    Console.Write("Select an option: ");
                    input = Console.ReadLine();
                }
                choice = Convert.ToInt32(input);

                //Switch case to handle the user's choice
                switch (choice)
                {
                    //Case for viewing the food queue
                    case 1:
                        Console.Clear();
                        ViewFoodQueue();
                        Console.Clear();
                        break;
                    //Case for exiting the kitchen menu
                    case 2:
                        return;
                    //Default case for handling invalid options
                    default:
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid option. Please try again.");
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                }
            }
        }

        //Method to view the food queue and handle order status updates
        private void ViewFoodQueue()
        {
            var database = Database.Instance;
            var orderQueue = database.GetOrderQueue();

            //Check if the order queue is empty
            if (orderQueue.Count == 0)
            {
                Console.WriteLine("No orders in the queue.");
            }
            else
            {

                //Loop through the order queue to display each order and its status
                foreach (var entry in orderQueue)
                {
                    var order = entry.Order;
                    var status = entry.Status;
                    var number = entry.Number;

                    //Set color based on order status
                    switch (status)
                    {
                        case OrderStatus.Start:
                            Console.ForegroundColor = ConsoleColor.Red;
                            break;
                        case OrderStatus.Preparing:
                            Console.ForegroundColor = ConsoleColor.Blue;
                            break;
                        case OrderStatus.Finish:
                            Console.ForegroundColor = ConsoleColor.Green;
                            break;
                    }

                    //Define box dimensions for displaying order details
                    int boxWidth = 30;
                    string horizontalLine = new string('=', boxWidth);

                    //Print order details in a formatted box
                    Console.WriteLine(horizontalLine);
                    PrintLine($"Order No: {number}", boxWidth);
                    PrintLine($"Status: {status}", boxWidth);
                    Console.WriteLine(new string('-', boxWidth));

                    int counter2 = 1;
                    foreach (var item in order.OrderItems)
                    {
                        PrintLine($"{counter2++}. {item.Name}", boxWidth);
                    }

                    Console.WriteLine(horizontalLine);
                    Console.WriteLine();
                    Console.WriteLine();

                    //Reset color to default
                    Console.ForegroundColor = ConsoleColor.White;
                }

                //Display options for updating order status or exiting
                Console.WriteLine("1. Update status");
                Console.WriteLine("2. Exit");
                Console.Write("Select an option: ");
                string? input = Console.ReadLine();
                while (string.IsNullOrWhiteSpace(input))
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid option. Please try again.");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                    return;
                }
                choice = Convert.ToInt32(input);

                //Switch case to handle the user's choice for updating order status or exiting
                switch (choice)
                {
                    //Case for updating the status of an order
                    case 1:
                        Console.WriteLine("Enter Order No: ");
                        int OrderNo = Convert.ToInt32(Console.ReadLine());
                        //Loop through the order queue to find and update the specified order
                        foreach (var entry in orderQueue)
                        {
                            if (entry.Number == OrderNo)
                            {
                                database.UpdateOrderStatus(entry.Order);
                                ViewFoodQueue();
                                break;
                            }
                        }
                        //If the order does not exist, display an error message
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Order Does not exist!");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        Console.Clear();
                        ViewFoodQueue();
                        break;

                    //Case for exiting the order status update menu
                    case 2:
                        return;

                    //Default case for handling invalid options
                    default:
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid option. Please try again.");
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                }
            }
        }

        //Method to print a line of text within a specified width box
        private static void PrintLine(string text, int width)
        {
            //If the text exceeds the box width, truncate it and add ellipsis
            if (text.Length > width - 2)
            {
                text = string.Concat(text.AsSpan(0, width - 5), "...");
            }
            //Format the text within a box and print it
            string line = $"| {text.PadRight(width - 3)}|";
            Console.WriteLine(line);
        }
    }
}
