using System;
using System.Collections.Generic;

namespace Group01RestaurantSystem.CommandCLI
{
    /// <summary>
    /// Abstract base class for commands in the restaurant system.
    /// </summary>
    public abstract class Command
    {
        // Dictionary to store predefined passwords for different roles.
        // It is marked as readonly because it should not be modified at runtime.
        private static readonly Dictionary<string, string> userPasswords = new Dictionary<string, string>
        {
            {"Manager", "managergo"}, {"FOHStaff", "staffgo"}, {"Chef", "chefgo"}
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="Command"/> class.
        /// </summary>
        protected Command()
        {
        }

        /// <summary>
        /// Abstract method to be implemented by derived classes to execute specific commands.
        /// </summary>
        public abstract void Execute();

        /// <summary>
        /// Static method to start the command-line interface for the restaurant system.
        /// </summary>
        public static void Start()
        {
            // Infinite loop to continuously prompt the user until they choose to exit.
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Welcome to The Relaxing Koala System");
                Console.WriteLine("Please select your role: (Guest, Manager, FOHStaff, Chef) or type 'Exit' to quit:");

                string? role = Console.ReadLine();

                // Check if the input is null or empty and prompt the user to try again.
                if (string.IsNullOrEmpty(role))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No input provided. Please try again.\n");
                    Console.ForegroundColor = ConsoleColor.White;
                    continue;
                }

                // Trim any extra whitespace from the input.
                role = role.Trim();

                // Check if the user wants to exit the program.
                if (role.Equals("exit", StringComparison.OrdinalIgnoreCase) || role.Equals("e", StringComparison.OrdinalIgnoreCase))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Exiting program...");
                    Console.ForegroundColor = ConsoleColor.White;
                    return;
                }

                // Check if the user selected the Guest role.
                if (role.Equals("guest", StringComparison.OrdinalIgnoreCase) || role.Equals("g", StringComparison.OrdinalIgnoreCase))
                {
                    Console.Clear();
                    CustomerInterface();
                }
                // Check if the selected role is valid and requires a password.
                else if (userPasswords.TryGetValue(role, out string? storedPassword))
                {
                    Console.WriteLine("Enter your password:");
                    string? password = Console.ReadLine();

                    if (password == storedPassword)
                    {
                        Console.Clear();
                        // Switch case to navigate to the appropriate interface based on the role.
                        switch (role)
                        {
                            case "Manager":
                                ManagerInterface();
                                break;

                            case "FOHStaff":
                                FOHStaffInterface();
                                break;

                            case "Chef":
                                ChefInterface();
                                break;
                        }
                    }
                    else
                    {
                        // Notify the user if the password is incorrect.
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Incorrect password, please try again.\n");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                else
                {
                    // Notify the user if an invalid role is selected.
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid role selected, please try again.\n");
                    Console.ForegroundColor = ConsoleColor.White;
                }

                Console.WriteLine("Press anything to continue");
                Console.ReadKey();
            }
        }

        private static void CustomerInterface()
        {
            Command orderCli = new OrderCLI();
            orderCli.Execute();
        }

        private static void ManagerInterface()
        {
            Command analyticsCLI = new AnalyticsCLI();
            analyticsCLI.Execute();
        }

        private static void FOHStaffInterface()
        {
            Command reservationCLI = new ReservationCLI();
            reservationCLI.Execute();
        }

        private static void ChefInterface()
        {
            Command kitchenCLI = new KitchenCLI();
            kitchenCLI.Execute();
        }
    }
}
