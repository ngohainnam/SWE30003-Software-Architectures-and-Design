﻿using System;

namespace Group01RestaurantSystem.CommandCLI
{
    /// <summary>
    /// Class to handle analytics command line interface operations, inheriting from the abstract Command class.
    /// </summary>
    internal class AnalyticsCLI : Command
    {
        private int choice;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnalyticsCLI"/> class.
        /// </summary>
        public AnalyticsCLI()
        {
        }

        /// <summary>
        /// Overridden Execute method to display and handle the analytics menu.
        /// </summary>
        public override void Execute()
        {
            Console.Clear();
            // Infinite loop to continuously display the menu until the user chooses to exit
            while (true)
            {
                // Display menu options
                Console.WriteLine("1. Sale Data");
                Console.WriteLine("2. Update Menu");
                Console.WriteLine("3. Exit");
                Console.Write("Select an option: ");

                // Read user input and try to parse it into an integer
                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    // If parsing fails, display an error message and prompt the user to try again
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid option. Please try again.");
                    Console.ForegroundColor = ConsoleColor.White;
                    continue;
                }

                switch (choice)
                {
                    // Case for viewing sales data
                    case 1:
                        DisplaySalesData();
                        break;

                    // Case for adjusting menu items
                    case 2:
                        AdjustMenu();
                        break;

                    // Case for exiting the analytics menu
                    case 3:
                        return;

                    // Default case for handling invalid options
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid option. Please try again.");
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                }
            }
        }

        /// <summary>
        /// Displays sales data.
        /// </summary>
        private void DisplaySalesData()
        {
            DateTime startDate, endDate;

            Console.Write("Enter the starting date (dd_MM_yy): ");
            string? startDateString = Console.ReadLine();
            // Validate the date format and prompt again if invalid
            while (!DateTime.TryParseExact(startDateString, "dd_MM_yy", null, System.Globalization.DateTimeStyles.None, out startDate))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid date format. Please enter the date in dd_MM_yy format.");
                Console.ForegroundColor = ConsoleColor.White;
                startDateString = Console.ReadLine();
            }

            Console.Write("Enter the ending date (dd_MM_yy): ");
            string? endDateString = Console.ReadLine();
            // Validate the date format and prompt again if invalid
            while (!DateTime.TryParseExact(endDateString, "dd_MM_yy", null, System.Globalization.DateTimeStyles.None, out endDate))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid date format. Please enter the date in dd_MM_yy format.");
                Console.ForegroundColor = ConsoleColor.White;
                endDateString = Console.ReadLine();
            }

            Database.Instance.ReadSalesData(startDate, endDate);

            Console.WriteLine("Press anything to continue");
            Console.ReadKey();
            Console.Clear();
        }

        /// <summary>
        /// Adjusts the menu.
        /// </summary>
        private void AdjustMenu()
        {
            Console.Clear();
            Menu menu = new Menu();
            Console.WriteLine("1. Delete Menu Item");
            Console.WriteLine("2. Update Price");
            Console.WriteLine("3. Input Menu");
            Console.WriteLine("4. Exit");
            Console.Write("Select an option: ");

            // Incorrect input handling
            if (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid option. Please try again.");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }

            switch (choice)
            {
                case 1:
                    DeleteMenuItem(menu);
                    break;
                case 2:
                    UpdateMenuItemPrice(menu);
                    break;
                case 3:
                    InputMenuItem(menu);
                    break;
                case 4:
                    return;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid option. Please try again.");
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }
        }

        /// <summary>
        /// Deletes a menu item.
        /// </summary>
        /// <param name="menu">The menu object.</param>
        private void DeleteMenuItem(Menu menu)
        {
            menu.PrintMenu();
            Console.WriteLine();
            Console.Write("Enter the index of the menu item you want to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int index) || index < 1 || index > menu.MenuItems.Count)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid index. Please try again.");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }

            List<MenuItem> menuItems = menu.MenuItems;
            menuItems.RemoveAt(index - 1);
            for (int i = 0; i < menuItems.Count; i++)
            {
                menuItems[i].Index = i + 1;
            }
            menu.MenuItems = menuItems;
            Database.Instance.WriteMenuItems(menuItems);

            Console.WriteLine("Menu item has been deleted");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }

        /// <summary>
        /// Updates the price of a menu item.
        /// </summary>
        /// <param name="menu">The menu object.</param>
        private void UpdateMenuItemPrice(Menu menu)
        {
            menu.PrintMenu();
            Console.WriteLine();
            Console.Write("Enter the index of the menu item you want to update: ");
            if (!int.TryParse(Console.ReadLine(), out int index) || index < 1 || index > menu.MenuItems.Count)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid index. Please try again.");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }

            Console.Write("Enter the new price: ");
            if (!double.TryParse(Console.ReadLine(), out double price))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid price. Please enter a valid number.");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }

            List<MenuItem> menuItems = menu.MenuItems;
            MenuItem menuItem = menuItems[index - 1];
            menuItem.Price = price;
            menu.MenuItems = menuItems;
            Database.Instance.WriteMenuItems(menuItems);

            Console.WriteLine("Menu item price has been updated");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }

        /// <summary>
        /// Inputs a new menu item.
        /// </summary>
        /// <param name="menu">The menu object.</param>
        private void InputMenuItem(Menu menu)
        {
            menu.PrintMenu();
            Console.WriteLine();
            Console.Write("Enter the name of the menu item: ");
            string name = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Input cannot be empty. Please enter a valid name.");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Console.Clear();
                return;
            }

            Console.Write("Enter the price of the menu item: ");
            if (!double.TryParse(Console.ReadLine(), out double price))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid price. Please enter a valid number.");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }

            Console.Write("Enter the description of the menu item: ");
            string description = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(description))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Input cannot be empty. Please enter a valid description.");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Console.Clear();
                return;
            }

            Console.Write("Enter the category of the menu item: ");
            string category = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(category))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Input cannot be empty. Please enter a valid category.");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Console.Clear();
                return;
            }

            List<MenuItem> menuItems = menu.MenuItems;
            menuItems.Add(new MenuItem(menuItems.Count + 1, name, price, description, category));
            menu.MenuItems = menuItems;
            Database.Instance.WriteMenuItems(menuItems);

            Console.WriteLine("Menu item has been added");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
