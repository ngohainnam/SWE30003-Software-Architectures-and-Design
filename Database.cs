using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Group01RestaurantSystem
{
    internal class Database
    {
        private Dictionary<int, Order> orders;
        private Dictionary<string, int> menuItemSales;
        private static Database? instance;
        private static readonly object lockObject = new object();

        public Database()
        {
            orders = new Dictionary<int, Order>();
            menuItemSales = new Dictionary<string, int>();
            ReadSales();
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
            order.CurrentDateTime = DateTime.Now;
            orders.Add(newOrderId, order);

            // Update sales data for each item in the order
            foreach (MenuItem item in order.GetOrderItems)
            {
                if (menuItemSales.ContainsKey(item.GetName))
                {
                    menuItemSales[item.GetName] += 1;  // Increment count for the item
                }
                else
                {
                    menuItemSales.Add(item.GetName, 1);  // Add new item with count 1
                }
            }
            SaveOrder();
            WriteSales();
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

        public void SaveOrder()
        {
            DateTime now = DateTime.Now;
            string formattedDateTime = now.ToString("yyyy-MM-dd_HH-mm-ss");
            string fileName = $"orders_{formattedDateTime}.json";
            string jsonString = JsonSerializer.Serialize(orders, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(fileName, jsonString);
            Console.WriteLine($"Data has been written to {fileName}");
        }

        public void ReadSales()
        {
            string salesFilePath = "sales_Data.json";


            // Check if the sales data file exists
            if (File.Exists(salesFilePath))
            {
                string salesJson = File.ReadAllText(salesFilePath);

                if (!string.IsNullOrWhiteSpace(salesJson))
                {
                    try
                    {
                        var deserializedSales = JsonSerializer.Deserialize<Dictionary<string, int>>(salesJson);
                        if (deserializedSales != null && deserializedSales.Count > 0)
                        {
                            menuItemSales = deserializedSales;
                            Console.WriteLine("Data has been read from sales_Data.json");
                        }
                        else
                        {
                            Console.WriteLine("sales_Data.json is empty. Initialized an empty sales dictionary.");
                        }
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"Failed to deserialize sales data: {ex.Message}");
                        menuItemSales = new Dictionary<string, int>(); // Fallback to an empty dictionary
                    }
                }
                else
                {
                    Console.WriteLine("sales_Data.json is empty. Initialized an empty sales dictionary.");
                }
            }
            else
            {
                Console.WriteLine("sales_Data.json does not exist. Initialized an empty sales dictionary.");
            }


        }
        public void WriteSales()
        {
            string salesFilePath = "sales_Data.json";
            string updatedSalesJson = JsonSerializer.Serialize(menuItemSales, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(salesFilePath, updatedSalesJson);
            Console.WriteLine("Sales data has been updated and written to sales_Data.json");
        }




    }
}
