using System;

namespace Group01RestaurantSystem.CommandCLI
{
    internal class analyticsCLI : Command
    {
        public analyticsCLI()
        {
        }

        public override void Execute()
        {
            Console.Clear();
            while (true)
            {
                Console.WriteLine("1. Sale Data");
                Console.WriteLine("2. Exit");
                Console.Write("Select an option: ");

                int choice;
                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid option. Please try again.");
                    Console.ForegroundColor = ConsoleColor.White;
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        DateTime startDate, endDate;

                        Console.Write("Enter the starting date (dd_MM_yy): ");
                        string? startDateString = Console.ReadLine();
                        while (!DateTime.TryParseExact(startDateString, "dd_MM_yy", null, System.Globalization.DateTimeStyles.None, out startDate))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid date format. Please enter the date in dd_MM_yy format.");
                            Console.ForegroundColor = ConsoleColor.White;
                            startDateString = Console.ReadLine();
                        }

                        Console.Write("Enter the ending date (dd_MM_yy): ");
                        string? endDateString = Console.ReadLine();
                        while (!DateTime.TryParseExact(endDateString, "dd_MM_yy", null, System.Globalization.DateTimeStyles.None, out endDate))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid date format. Please enter the date in dd_MM_yy format.");
                            Console.ForegroundColor = ConsoleColor.White;
                            endDateString = Console.ReadLine();
                        }

                        Database.Instance.ReadSalesData(startDate, endDate);
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
