using System;
using System.Collections.Generic;

namespace Group01RestaurantSystem
{
    /// <summary>
    /// Represents an order in the restaurant system.
    /// </summary>
    internal class Order
    {
        /// <summary>
        /// Gets or sets the list of items in the order.
        /// </summary>
        public List<MenuItem> OrderItems { get; set; }

        /// <summary>
        /// Gets or sets the total price of the order.
        /// </summary>
        public double Total { get; set; }

        /// <summary>
        /// Gets or sets the current date and time when the order was created.
        /// </summary>
        public DateTime CurrentDateTime { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Order"/> class.
        /// </summary>
        public Order()
        {
            CurrentDateTime = DateTime.Now;
            OrderItems = new List<MenuItem>();
            Total = 0;
        }

        /// <summary>
        /// Adds an item to the order and updates the total price.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public void AddItem(MenuItem item)
        {
            OrderItems.Add(item);
            Total += item.Price;  // Update total price whenever an item is added.
        }

        /// <summary>
        /// Removes an item from the order and updates the total price.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        public void RemoveItem(MenuItem item)
        {
            if (OrderItems.Contains(item))
            {
                OrderItems.Remove(item);
                Total -= item.Price;  // Update total price whenever an item is removed.
            }
        }

        /// <summary>
        /// Prints the current order and total price to the console.
        /// </summary>
        public void PrintOrder()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Current Order:");
            int i = 1;
            foreach (var item in OrderItems)
            {
                Console.WriteLine($"{i} - {item.Name} - {item.Price:C}");
                i++;
            }
            Console.WriteLine($"Total: {Total:C}");
            Console.ForegroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// Gets the total price of the order.
        /// </summary>
        /// <returns>The total price.</returns>
        public double GetTotal()
        {
            return Total;
        }

        /// <summary>
        /// Gets an order item by its index.
        /// </summary>
        /// <param name="index">The index of the item.</param>
        /// <returns>The order item.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown when the index is out of range.</exception>
        public MenuItem GetOrderItem(int index)
        {
            if (index >= 0 && index < OrderItems.Count)
            {
                return OrderItems[index];
            }
            else
            {
                throw new IndexOutOfRangeException("Index out of range.");
            }
        }
    }
}
