using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group01RestaurantSystem
{
    internal class OrderQueueEntry
    {
        //Define variables needed
        public Order Order { get; set; }
        public OrderStatus Status { get; set; }
        public int Number { get; set; }

        //Making a constructor
        public OrderQueueEntry(Order order, OrderStatus status, int number)
        {
            Order = order;
            Status = status;
            Number = number;
        }
    }
}
