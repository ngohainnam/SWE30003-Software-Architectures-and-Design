using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group01RestaurantSystem
{
    internal class OrderQueueEntry
    {
        public Order Order { get; set; }
        public OrderStatus Status { get; set; }

        public OrderQueueEntry(Order order, OrderStatus status)
        {
            Order = order;
            Status = status;
        }
    }
}
