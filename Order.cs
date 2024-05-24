using System;
using System.Collections.Generic;

namespace Group01RestaurantSystem
{
    internal class Order
    {
        public List<MenuItem> OrderItems { get; set; }
        public double Total { get; set; }
        public DateTime CurrentDateTime { get; set; }

        public Order()
        {
            CurrentDateTime = DateTime.Now;
            OrderItems = new List<MenuItem>();
            Total = 0;
        }

        //Method to add an item to the order
        public void AddItem(MenuItem item)
        {
            OrderItems.Add(item);
            Total += item.Price;  //Update total price whenever an item is added
        }

        //Method to remove an item from the order
        public void RemoveItem(MenuItem item)
        {
            if (OrderItems.Contains(item))
            {
                OrderItems.Remove(item);
                Total -= item.Price;  //Update total price whenever an item is removed
            }
        }

        //Method to print the current order and total
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

        //Method to get the total price of the order
        public double GetTotal()
        {
            return Total;
        }

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
