using System;
using Group01RestaurantSystem.CommandCLI;
using Group01RestaurantSystem.Transaction;
using Group01RestaurantSystem.Roles;

namespace Group01RestaurantSystem
{
    public class Program
    {
        public static void Main()
        {
            //create a list of string
            List<string> commandList = new List<string>();
            commandList.Add("Create Reservation");
            commandList.Add("View Reservation");
            commandList.Add("Update Reservation");

            Command OrderCLI = new orderCLI(commandList);
            OrderCLI.PrintCommand();


            //Initialize FOHstaff
            //Initialize Chef
            //Initialize Database
            //Create Instances of reservationCLI, orderCLI, kitchenCLI, analyticsCLI
            //
        }
    }
}