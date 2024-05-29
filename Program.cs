using System;
using Group01RestaurantSystem.CommandCLI;
using Group01RestaurantSystem.Transaction;

namespace Group01RestaurantSystem
{
    /// <summary>
    /// Main entry point for the restaurant system application.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The main method that starts the application.
        /// </summary>
        public static void Main()
        {
            Command.Start();
        }
    }
}
