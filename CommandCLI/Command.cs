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
        private List<string> CommandList;

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
            bool isAuthenticated = false;
            while (!isAuthenticated)
            {
                Console.WriteLine("Welcome to The Relaxing Koala System");
                Console.WriteLine("Please select your role: (Guest, Manager, FOHStaff, Chef) or type 'Exit' to quit:");
                string? role = Console.ReadLine();

                if (string.IsNullOrEmpty(role))
                {
                    Console.WriteLine("No input provided. Please try again.\n");
                    continue; 
                }

                role = role.Trim(); 

                if (role.Equals("Exit"))
                {
                    Console.WriteLine("Exiting program...");
                    return; 
                }

                if (role.Equals("Guest"))
                {
                    ClearScreen();
                    GuestInterface();
                    isAuthenticated = true; 
                }
                else if (userPasswords.TryGetValue(role, out string? storedPassword))
                {
                    Console.WriteLine("Enter your password:");
                    string? password = Console.ReadLine();

                    if (password == storedPassword)
                    {
                        ClearScreen();
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
                        isAuthenticated = true; 
                    }
                    else
                    {
                        Console.WriteLine("Incorrect password, please try again.\n");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid role selected, please try again.\n");
                }
            }
        }

        private static void ClearScreen()
        {
            Console.Clear();
        }

        private static void GuestInterface()
        {
            Console.WriteLine("Welcome, Guest! Here are your option:");
            Command orderCli = new orderCLI(new List<string>()); 

            orderCli.Execute();
        }

        private static void ManagerInterface()
        {
            Console.WriteLine("Welcome, Manager!");
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
