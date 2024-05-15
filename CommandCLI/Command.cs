using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group01RestaurantSystem.CommandCLI
{
    public abstract class Command
    {
        private int userChoice;
        private readonly List<string> CommandList;
        private readonly Database database = new Database();

        // Make the dictionary readonly since it is not modified at runtime
        private static readonly Dictionary<string, string> userPasswords = new Dictionary<string, string>
        {
            {"Manager", "managergo"}, {"FOHStaff", "staffgo"}, {"Chef", "chefgo"}
        };

        public Command(List<string> commandList)
        {
            // Direct assignment from the parameter
            CommandList = commandList ?? new List<string>(); // Safeguard against null input
        }

        public int UserChoice
        {
            get
            {
                return userChoice;
            }
            set
            {
                userChoice = value;
            }
        }

        public abstract void Execute();

        public void PrintCommand()
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

                role = role.Trim(); 

                if (role.Equals("Exit"))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Exiting program...");
                    Console.ForegroundColor = ConsoleColor.White;
                    return; 
                }

                if (role.Equals("Guest"))
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
            Command orderCli = new orderCLI(new List<string>()); 
            orderCli.Execute();
        }

        private static void ManagerInterface()
        {
            Console.Clear();
            while (true)
            {
                Console.WriteLine("1. Sale Data");
                Console.WriteLine("2. Exit");
                Console.Write("Select an option: ");

                int choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        Database.Instance.PrintSalesData();
                        break;
                    case 2:
                        return; 
                }
            }
        }

        private static void FOHStaffInterface()
        {
            Console.WriteLine("Welcome FOH Staff!");
        }

        private static void ChefInterface()
        {
            Console.WriteLine("Welcome Chef!");
        }
    }
}
