using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;

namespace Group01RestaurantSystem
{
    public class Table
    {
        private readonly int tableNo;
        private bool reserved;
        private string customerName;

        public Table(int number)
        {
            this.tableNo = number;
            this.reserved = false;
            this.customerName = "";
        }

        public int GetTableNo
        {
            get { return tableNo; }
        }

        public bool Reserve
        {
            get { return reserved; }
            set { reserved = value; }
        }

        public string CustomerName
        {
            get { return customerName; }
            set { customerName = value; }
        }
    }
}
