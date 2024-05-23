using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Group01RestaurantSystem
{
    enum OrderStatus
    {
        Start,
        Preparing,
        Finish
    }

    internal class Database
    {
        private List<Order> orders;
        private List<OrderQueueEntry> orderQueue;
        private Dictionary<string, int> menuItemSales;
        private static Database? instance;
        private static readonly object lockObject = new object();
        private List<Table> tables;

        public Database()
        {
            orders = new List<Order>();
            orderQueue = new List<OrderQueueEntry>();
            menuItemSales = new Dictionary<string, int>();
            tables = new List<Table>();
            ReadSales();
            ReadOrderQueue();
            DateTime now = DateTime.Now;
            string formattedDate = now.ToString("dd_MM_yy");
            ReadOrders(formattedDate);
            ReadReservation();
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

        public List<Table> Tables => tables;

        public List<OrderQueueEntry> GetOrderQueue()
        {
            return orderQueue;
        }

        // Method to add an order to the database
        public void AddOrder(Order order)
        {
            order.CurrentDateTime = DateTime.Now;
            orders.Add(order);
            orderQueue.Add(new OrderQueueEntry(order, OrderStatus.Start));
            WriteOrderQueue();
            // Update sales data for each item in the order
            foreach (MenuItem item in order.OrderItems)
            {
                if (menuItemSales.ContainsKey(item.Name))
                {
                    menuItemSales[item.Name] += 1;  // Increment count for the item
                }
                else
                {
                    menuItemSales.Add(item.Name, 1);  // Add new item with count 1
                }
            }
            SaveOrder();
            WriteSales();
        }

        public void UpdateOrderStatus(Order order)
        {
            var entry = orderQueue.FirstOrDefault(e => e.Order == order);
            if (entry != null)
            {
                switch (entry.Status)
                {
                    case OrderStatus.Start:
                        entry.Status = OrderStatus.Preparing;
                        break;
                    case OrderStatus.Preparing:
                        entry.Status = OrderStatus.Finish;
                        break;
                    case OrderStatus.Finish:
                        orderQueue.Remove(entry);
                        break;
                }
                WriteOrderQueue(); // Ensure the order queue is written after updating
            }
        }

        public void ReadOrderQueue()
        {
            string fileName = $"OrderQueue/OrderQueue.json";
            if (File.Exists(fileName))
            {
                string jsonString = File.ReadAllText(fileName);
                var deserializedData = JsonSerializer.Deserialize<List<OrderQueueEntry>>(jsonString);
                if (deserializedData != null)
                {
                    orderQueue = deserializedData;
                }
            }
        }

        public void WriteOrderQueue()
        {
            if (!Directory.Exists("OrderQueue"))
            {
                Directory.CreateDirectory("OrderQueue");
            }
            string fileName = $"OrderQueue/OrderQueue.json";
            string jsonString = JsonSerializer.Serialize(orderQueue, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(fileName, jsonString);
            Console.WriteLine($"Data has been written to {fileName}");
        }

        public void SaveOrder()
        {
            if (!Directory.Exists("Orders"))
            {
                Directory.CreateDirectory("Orders");
            }
            DateTime now = DateTime.Now;
            string formattedDate = now.ToString("dd_MM_yy");
            string fileName = $"Orders/orders_{formattedDate}.json";
            string jsonString = JsonSerializer.Serialize(orders, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(fileName, jsonString);
            Console.WriteLine($"Data has been written to {fileName}");
        }

        public void ReadOrders(string date)
        {
            string fileName = $"Orders/orders_{date}.json";
            if (File.Exists(fileName))
            {
                string jsonString = File.ReadAllText(fileName);
                var deserializedOrders = JsonSerializer.Deserialize<List<Order>>(jsonString);
                if (deserializedOrders != null)
                {
                    orders = deserializedOrders;
                }
                else
                {
                    Console.WriteLine($"No orders found for {date}");
                }
            }
            else
            {
                Console.WriteLine($"Order file for {date} does not exist.");
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

        public void SaveReservation()
        {
            string reservationFilePath = "reservation_Data.json";
            string updatedReservationJson = JsonSerializer.Serialize(tables, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(reservationFilePath, updatedReservationJson);
            Console.WriteLine("Reservation data has been updated and written to reservation_Data.json");
        }

        public void ReadReservation()
        {
            string reservationFilePath = "reservation_Data.json";
            if (File.Exists(reservationFilePath))
            {
                string reservationJson = File.ReadAllText(reservationFilePath);

                if (!string.IsNullOrWhiteSpace(reservationJson))
                {
                    try
                    {
                        var deserializedReservation = JsonSerializer.Deserialize<List<Table>>(reservationJson);
                        if (deserializedReservation != null && deserializedReservation.Count > 0)
                        {
                            tables = deserializedReservation;
                        }
                        else
                        {
                            Console.WriteLine("reservation_Data.json is empty. Initialized an empty reservation list.");
                        }
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"Failed to deserialize reservation data: {ex.Message}");
                        tables = new List<Table>(); // Fallback to an empty list
                    }
                }
                else
                {
                    Console.WriteLine("reservation_Data.json is empty. Initialized an empty reservation list.");
                }
            }
            else
            {
                Console.WriteLine("reservation_Data.json does not exist. Initialized an empty reservation list.");
            }
        }
    }
}
