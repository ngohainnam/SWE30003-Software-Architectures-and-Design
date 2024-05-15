using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group01RestaurantSystem
{
    internal class Database
    {
        private readonly Dictionary<int, Order> orders;
        private readonly Dictionary<string, int> menuItemSales;
        private static Database? instance;
        private static readonly object lockObject = new object();

        public Database()
        {
            orders = new Dictionary<int, Order>();
            menuItemSales = new Dictionary<string, int>();
        }

        // Singleton pattern 
        public static Database Instance
        {
            get
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        instance = new Database();
                    }
                    return instance;
                }
            }
        }

        // Method to add an order to the database
        public void AddOrder(Order order)
        {
            int newOrderId = orders.Count + 1;  // Simple order ID generation
            orders.Add(newOrderId, order);

            // Update sales data for each item in the order
            foreach (MenuItem item in order.GetOrderItems())
            {
                if (menuItemSales.ContainsKey(item.GetName()))
                {
                    menuItemSales[item.GetName()] += 1;  // Increment count for the item
                }
                else
                {
                    menuItemSales.Add(item.GetName(), 1);  // Add new item with count 1
                }
            }
        }

        // Method to print sales data for each menu item
        public void PrintSalesData()
        {
            Console.WriteLine("Sales Data for Menu Items:");
            foreach (KeyValuePair<string, int> item in menuItemSales)
            {
                Console.WriteLine($"{item.Key}: {item.Value} sold");
            }
            Console.WriteLine('\n');
        }
    }
}
