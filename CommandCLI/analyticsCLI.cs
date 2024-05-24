using System;

namespace Group01RestaurantSystem.CommandCLI
{
    //Class to handle analytics command line interface operations, inheriting from the abstract Command class
    internal class analyticsCLI : Command
    {
        //Variable to store the user's menu choice
        private int choice;

        //Constructor for analyticsCLI class
        public analyticsCLI()
        {
        }

        //Overridden Execute method to display and handle the analytics menu
        public override void Execute()
        {
            //Clear the console screen
            Command.ClearScreen();
            //Infinite loop to continuously display the menu until the user chooses to exit
            while (true)
            {
                //Display menu options
                Console.WriteLine("1. Sale Data");
                Console.WriteLine("2. Exit");
                Console.Write("Select an option: ");

                //Read user input and try to parse it into an integer
                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    //If parsing fails, display an error message and prompt the user to try again
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid option. Please try again.");
                    Console.ForegroundColor = ConsoleColor.White;
                    continue;
                }

                //Switch case to handle the user's choice
                switch (choice)
                {
                    //Case for viewing sales data
                    case 1:
                        DateTime startDate, endDate;

                        //Prompt the user to enter the starting date
                        Console.Write("Enter the starting date (dd_MM_yy): ");
                        string? startDateString = Console.ReadLine();
                        //Validate the date format and prompt again if invalid
                        while (!DateTime.TryParseExact(startDateString, "dd_MM_yy", null, System.Globalization.DateTimeStyles.None, out startDate))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid date format. Please enter the date in dd_MM_yy format.");
                            Console.ForegroundColor = ConsoleColor.White;
                            startDateString = Console.ReadLine();
                        }

                        //Prompt the user to enter the ending date
                        Console.Write("Enter the ending date (dd_MM_yy): ");
                        string? endDateString = Console.ReadLine();
                        //Validate the date format and prompt again if invalid
                        while (!DateTime.TryParseExact(endDateString, "dd_MM_yy", null, System.Globalization.DateTimeStyles.None, out endDate))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid date format. Please enter the date in dd_MM_yy format.");
                            Console.ForegroundColor = ConsoleColor.White;
                            endDateString = Console.ReadLine();
                        }

                        //Call the database instance to read sales data for the specified date range
                        Database.Instance.ReadSalesData(startDate, endDate);

                        //Prompt the user to press any key to continue
                        Console.WriteLine("Press anything to continue");
                        Console.ReadKey();
                        Command.ClearScreen();
                        break;

                    //Case for exiting the analytics menu
                    case 2:
                        return;

                    //Default case for handling invalid options
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
