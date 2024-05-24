using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group01RestaurantSystem.CommandCLI
{
    //Abstract base class for commands in the restaurant system
    public abstract class Command
    {
        //Dictionary to store predefined passwords for different roles

        //It is marked as readonly because it should not be modified at runtime
        private static readonly Dictionary<string, string> userPasswords = new Dictionary<string, string>
        {
            {"Manager", "managergo"}, {"FOHStaff", "staffgo"}, {"Chef", "chefgo"}
        };

        //Default constructor for the Command class
        public Command()
        {
        }

        //Abstract method to be implemented by derived classes to execute specific commands
        public abstract void Execute();

        //Static method to start the command-line interface for the restaurant system
        public static void Start()
        {
            //Infinite loop to continuously prompt the user until they choose to exit
            while (true)
            {
                //Clear the console screen and display the welcome message
                ClearScreen();
                Console.WriteLine("Welcome to The Relaxing Koala System");
                Console.WriteLine("Please select your role: (Guest, Manager, FOHStaff, Chef) or type 'Exit' to quit:");

                //Read the user's input for their role
                string? role = Console.ReadLine();

                //Check if the input is null or empty and prompt the user to try again
                if (string.IsNullOrEmpty(role))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No input provided. Please try again.\n");
                    Console.ForegroundColor = ConsoleColor.White;
                    continue;
                }

                //Trim any extra whitespace from the input
                role = role.Trim();

                //Check if the user wants to exit the program
                if (role.Equals("exit") || role.Equals("Exit") || role.Equals("e"))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Exiting program...");
                    Console.ForegroundColor = ConsoleColor.White;
                    return;
                }

                //Check if the user selected the Guest role
                if (role.Equals("guest") || role.Equals("Guest") || role.Equals("g"))
                {
                    ClearScreen();
                    CustomerInterface();
                }
                //Check if the selected role is valid and requires a password
                else if (userPasswords.TryGetValue(role, out string? storedPassword))
                {
                    Console.WriteLine("Enter your password:");
                    string? password = Console.ReadLine();

                    //Verify the entered password
                    if (password == storedPassword)
                    {
                        ClearScreen();
                        //Switch case to navigate to the appropriate interface based on the role
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
                        //Notify the user if the password is incorrect
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Incorrect password, please try again.\n");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                else
                {
                    //Notify the user if an invalid role is selected
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid role selected, please try again.\n");
                    Console.ForegroundColor = ConsoleColor.White;
                }

                //Prompt the user to press any key to continue
                Console.WriteLine("Press anything to continue");
                Console.ReadKey();
            }
        }

        //Method to display the customer interface
        private static void CustomerInterface()
        {
            Command orderCli = new orderCLI();
            orderCli.Execute();
        }

        //Method to display the manager interface
        private static void ManagerInterface()
        {
            Command analyticsCLI = new analyticsCLI();
            analyticsCLI.Execute();
        }

        //Method to display the front-of-house staff interface
        private static void FOHStaffInterface()
        {
            Command reservationCLI = new reservationCLI();
            reservationCLI.Execute();
        }

        //Method to display the chef interface
        private static void ChefInterface()
        {
            Command kitchenCLI = new kitchenCLI();
            kitchenCLI.Execute();
        }

        //Method for clear screen (if we have better solution to clear the screen, just need to put it here)
        public static void ClearScreen()
        {
            Console.Clear();
        }

    }
}
