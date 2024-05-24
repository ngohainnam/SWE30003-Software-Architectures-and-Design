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
                Console.WriteLine("2. update menu");
                Console.WriteLine("3. Exit");
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
                    //Case for adjusting menu items
                    case 2:
                        Console.Clear();
                        Menu menu = new Menu();
                        Console.WriteLine("1.delete menu item");
                        Console.WriteLine("2.update price");
                        Console.WriteLine("3.input menu");
                        Console.WriteLine("4.Exit");
                        Console.Write("Select an option: ");
                        // incorrect input handling
                        if (!int.TryParse(Console.ReadLine(), out choice))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid option. Please try again.");
                            Console.ForegroundColor = ConsoleColor.White;
                            continue;
                        }
                        Console.Clear();
                        if(choice == 1)
                        {
                            menu.PrintMenu();
                            Console.WriteLine();
                            Console.WriteLine("Enter the index of the menu item you want to delete: ");
                            if (!int.TryParse(Console.ReadLine(), out int index1) || index1 < 1 || index1 > menu.getMenuItems.Count)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Invalid index. Please try again.");
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                            else
                            {
                                List<MenuItem> menuItems1 = menu.getMenuItems;
                                menuItems1.RemoveAt(index1 - 1);
                                for (int i = 0; i < menuItems1.Count; i++)
                                {
                                    menuItems1[i].Index = i + 1;
                                }
                                menu.setMenuItems = menuItems1;
                                Database.Instance.WriteMenuItems(menuItems1);
                            }
                            Console.WriteLine("menu item has been deleted");

                            Console.WriteLine("Press any key to continue...");
                            Console.ReadKey();
                            Command.ClearScreen(); //Clear the console screen
                        }
                        else if(choice == 2)
                        {
                            menu.PrintMenu();
                            Console.WriteLine();
                            Console.WriteLine("Enter the index of the menu item you want to update: ");
                            if (!int.TryParse(Console.ReadLine(), out int index2) || index2 < 1 || index2 > menu.getMenuItems.Count)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Invalid index. Please try again.");
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                            else
                            {
                                Console.WriteLine("Enter the new price: ");
                                if (!double.TryParse(Console.ReadLine(), out double price))
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Invalid price. Please enter a valid number.");
                                    Console.ForegroundColor = ConsoleColor.White;
                                }
                                else
                                {
                                    List<MenuItem> menuItems2 = menu.getMenuItems;
                                    MenuItem mUpdate = menuItems2[index2 - 1];
                                    mUpdate.Price = price;
                                    menu.setMenuItems = menuItems2;
                                    Database.Instance.WriteMenuItems(menuItems2);
                                }
                            }
                            Console.WriteLine("menu item Price has been update");

                            Console.WriteLine("Press any key to continue...");
                            Console.ReadKey();
                            Command.ClearScreen(); //Clear the console screen
                        }
                        else if (choice == 3)
                        {
                            menu.PrintMenu();
                            Console.WriteLine();
                            Console.WriteLine("Enter the name of the menu item: ");
                            string nameItem = Console.ReadLine() ?? "";
                            if (string.IsNullOrWhiteSpace(nameItem))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Input cannot be empty. Please enter a valid index.");
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine("Press any key to continue...");
                                Console.ReadKey();
                                Command.ClearScreen(); //Clear the console screen
                                continue;
                            }
                            else
                            {

                                Console.WriteLine("Enter the price of the menu item: ");
                                if (!double.TryParse(Console.ReadLine(), out double priceItem))
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Invalid price. Please enter a valid number.");
                                    Console.ForegroundColor = ConsoleColor.White;
                                }
                                else
                                {
                                    Console.WriteLine("Enter the description of the menu item: ");
                                    string descriptionItem = Console.ReadLine() ?? "";
                                    if (string.IsNullOrWhiteSpace(descriptionItem))
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("Input cannot be empty. Please enter a valid index.");
                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.WriteLine("Press any key to continue...");
                                        Console.ReadKey();
                                        Command.ClearScreen(); //Clear the console screen
                                        continue;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Enter the category of the menu item: ");
                                        string categoryItem = Console.ReadLine() ?? "";
                                        if (string.IsNullOrWhiteSpace(categoryItem))
                                        {
                                            Console.ForegroundColor = ConsoleColor.Red;
                                            Console.WriteLine("Input cannot be empty. Please enter a valid index.");
                                            Console.ForegroundColor = ConsoleColor.White;
                                            Console.WriteLine("Press any key to continue...");
                                            Console.ReadKey();
                                            Command.ClearScreen(); //Clear the console screen
                                            continue;
                                        }
                                        else
                                        {
                                            List<MenuItem> menuItems3 = menu.getMenuItems;
                                            menuItems3.Add(new MenuItem(menuItems3.Count + 1, nameItem, priceItem, descriptionItem, categoryItem));
                                            menu.setMenuItems = menuItems3;
                                            Database.Instance.WriteMenuItems(menuItems3);
                                        }
                                    }
                                }
                            }
                            Console.WriteLine("menu item has been added");

                            Console.WriteLine("Press any key to continue...");
                            Console.ReadKey();
                            Command.ClearScreen(); //Clear the console screen
                        }
                        else if (choice == 4)
                        {
                            return;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid option. Please try again.");
                            Console.ForegroundColor = ConsoleColor.White;
                        }   
                        break;

                    //Case for exiting the analytics menu
                    case 3:
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
