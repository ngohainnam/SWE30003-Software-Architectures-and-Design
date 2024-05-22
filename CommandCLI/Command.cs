using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group01RestaurantSystem.CommandCLI
{
    public abstract class Command
    {
        // Make the dictionary readonly since it is not modified at runtime
        private static readonly Dictionary<string, string> userPasswords = new Dictionary<string, string>
        {
            {"Manager", "managergo"}, {"FOHStaff", "staffgo"}, {"Chef", "chefgo"}
        };

        public Command()
        {
        }

        public abstract void Execute();

        public static void Start()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Welcome to The Relaxing Koala System");
                Console.WriteLine("Please select your role: (Guest, Manager, FOHStaff, Chef) or type 'Exit' to quit:");
                string? role = Console.ReadLine();

                if (string.IsNullOrEmpty(role))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No input provided. Please try again.\n");
                    Console.ForegroundColor = ConsoleColor.White;
                    continue;
                }

                role = role.Trim().ToLower();

                if (role.Equals("exit") || role.Equals("e"))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Exiting program...");
                    Console.ForegroundColor = ConsoleColor.White;
                    return;
                }

                if (role.Equals("guest") || role.Equals("g"))
                {
                    Console.Clear();
                    CustomerInterface();
                }
                else if (userPasswords.TryGetValue(role, out string? storedPassword))
                {
                    Console.WriteLine("Enter your password:");
                    string? password = Console.ReadLine();

                    if (password == storedPassword)
                    {
                        Console.Clear();
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
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Incorrect password, please try again.\n");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                else if (role.Equals("manager") || role.Equals("m") || role.Equals("fohstaff") || role.Equals("f") || role.Equals("chef") || role.Equals("c"))
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine("That role has not been implemented yet");
                    Console.BackgroundColor = ConsoleColor.Black;

                }
                else
                {
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
            Command orderCli = new orderCLI();
            orderCli.Execute();
        }

        private static void ManagerInterface()
        {
            Command analyticsCLI = new analyticsCLI();
            analyticsCLI.Execute();
        }

        private static void FOHStaffInterface()
        {
            // Create one instance of reservationCLI throughout entire project runtime.
            Command reservationCLI = new reservationCLI();
            reservationCLI.Execute();
        }

        private static void ChefInterface()
        {
            Command kitchenCLI = new kitchenCLI();
            kitchenCLI.Execute();
        }
    }
}
