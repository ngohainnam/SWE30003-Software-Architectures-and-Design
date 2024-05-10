using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group01RestaurantSystem
{
    public abstract class Command
    {
        private int userChoice;
        private List<string> CommandList;
        public Command(List<string> commandList)
        {
            CommandList = new List<string>();
            CommandList = commandList;
        }

        public abstract void Execute();
        public void PrintCommand()
        {
            for (int i = 0; i < CommandList.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {CommandList[i]}");
            }
            Console.WriteLine("Please enter your choice: ");
            //check for inval input
            userChoice = Convert.ToInt32(Console.ReadLine());
            while (!int.TryParse(Console.ReadLine(), out userChoice) || userChoice < 1 || userChoice > CommandList.Count)
            {
                Console.WriteLine("Invalid input, please enter a number between 1 and " + CommandList.Count);
                userChoice = Convert.ToInt32(Console.ReadLine());
            }
        }
    }
}
