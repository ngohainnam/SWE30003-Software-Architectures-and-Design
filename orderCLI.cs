using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group01RestaurantSystem
{
    internal class orderCLI : Command
    {
        public orderCLI(List<string> commandList) : base(commandList)
        {
        }

        public override void Execute()
        {
            //print menu
        }
    }
}
