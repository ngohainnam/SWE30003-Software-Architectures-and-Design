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
                        Console.Clear();
                        ViewFoodQueue();
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

        private void ViewFoodQueue()
        {
            var database = Database.Instance;
            var orderQueue = database.GetOrderQueue();

            if (orderQueue.Count == 0)
            {
                Console.WriteLine("No orders in the queue.");
            }
            else
            {
                int counter1 = 1;

                foreach (var entry in orderQueue)
                {
                    var order = entry.Key;
                    var status = entry.Value;

                    // Set color based on status
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

                    // Box dimensions
                    int boxWidth = 30;
                    string horizontalLine = new string('=', boxWidth);

                    Console.WriteLine(horizontalLine);
                    PrintLine($"Order No: {counter1++}", boxWidth);
                    PrintLine($"Status: {status}", boxWidth);
                    Console.WriteLine(new string('-', boxWidth));

                    int counter2 = 1;
                    foreach (var item in order.GetOrderItems)
                    {
                        PrintLine($"{counter2++}. {item.GetName}", boxWidth);
                    }

                    Console.WriteLine(horizontalLine);
                    Console.WriteLine();
                    Console.WriteLine();

                    // Reset color to default
                    Console.ForegroundColor = ConsoleColor.White;
                }

                Console.WriteLine("1. Update status");
                Console.WriteLine("2. Exit");
                Console.Write("Select an option: ");
                int choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        Console.WriteLine("Enter Order No: ");
                        int OrderNo = Convert.ToInt32(Console.ReadLine());
                        counter1 = 1;
                        // Loop through the order queue to find the order
                        foreach (var entry1 in orderQueue)
                        {
                            if (counter1++ == OrderNo)
                            {
                                database.UpdateOrderStatus(entry1.Key);
                                ViewFoodQueue();
                                break;
                            }
                        }
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Order Does not exist!");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadLine();
                        Console.Clear();
                        ViewFoodQueue();
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

        private void PrintLine(string text, int width)
        {
            if (text.Length > width - 2)
            {
                text = text.Substring(0, width - 5) + "...";
            }
            string line = $"| {text.PadRight(width - 3)}|";
            Console.WriteLine(line);
        }
    }
}
