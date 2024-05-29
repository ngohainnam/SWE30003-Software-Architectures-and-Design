using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group01RestaurantSystem
{
    /// <summary>
    /// Represents an entry in the order queue.
    /// </summary>
    internal class OrderQueueEntry
    {
        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        public Order Order { get; set; }

        /// <summary>
        /// Gets or sets the status of the order.
        /// </summary>
        public OrderStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the order number.
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderQueueEntry"/> class.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <param name="status">The status of the order.</param>
        /// <param name="number">The order number.</param>
        public OrderQueueEntry(Order order, OrderStatus status, int number)
        {
            Order = order;
            Status = status;
            Number = number;
        }
    }
}
