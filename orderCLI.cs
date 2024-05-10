using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group01RestaurantSystem
{
    internal class orderCLI : Command
    {
        private Menu fMenu;
        public orderCLI(List<string> commandList) : base(commandList)
        {
            fMenu = new Menu();
        }

        public override void Execute()
        {
            if (UserChoice == 1 )
            {
                fMenu.PrintMenu();
            }
        }
    }
}
