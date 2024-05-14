using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group01RestaurantSystem
{
    internal class Order
    {
        private List<MenuItem> orderItems;
        private double total;

        public Order()
        {
            orderItems = new List<MenuItem>();
            total = 0;
        }

        // Method to add an item to the order
        public void AddItem(MenuItem item)
        {
            orderItems.Add(item);
            total += item.GetPrice();  // Update total price whenever an item is added
        }

        // Method to remove an item from the order
        public void RemoveItem(MenuItem item)
        {
            if (orderItems.Contains(item))
            {
                orderItems.Remove(item);
                total -= item.GetPrice();  // Update total price whenever an item is removed
            }
        }

        // Method to print the current order and total
        public void PrintOrder()
        {
            Console.WriteLine("Current Order:");
            foreach (var item in orderItems)
            {
                Console.WriteLine($"{item.GetName()} - {item.GetPrice():C}");
            }
            Console.WriteLine($"Total: {total:C}");
        }

        // Method to get the total price of the order
        public double GetTotal()
        {
            return total;
        }
    }
}
