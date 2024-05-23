using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
        private static Database? instance;
        private static readonly object lockObject = new object();
        private List<Table> tables;

        public Database()
        {
            orders = new List<Order>();
            orderQueue = new List<OrderQueueEntry>();
            tables = new List<Table>();
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
            SaveOrder();
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
        }

        public void SaveReservation()
        {
            string reservationFilePath = "reservation_Data.json";
            string updatedReservationJson = JsonSerializer.Serialize(tables, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(reservationFilePath, updatedReservationJson);
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

        public void ReadSalesData(DateTime startDate, DateTime endDate)
        {
            DateTime stDate = startDate;
            DateTime etDate = endDate;

            Dictionary<string, (double Price, int Quantity)> aggregatedSalesData = new Dictionary<string, (double, int)>();

            while (stDate <= etDate)
            {
                string fileName = $"Orders/orders_{stDate:dd_MM_yy}.json";
                if (File.Exists(fileName))
                {
                    string jsonString = File.ReadAllText(fileName);
                    var deserializedOrders = JsonSerializer.Deserialize<List<Order>>(jsonString);
                    if (deserializedOrders != null)
                    {
                        foreach (var order in deserializedOrders)
                        {
                            foreach (var item in order.OrderItems)
                            {
                                if (aggregatedSalesData.ContainsKey(item.Name))
                                {
                                    var currentData = aggregatedSalesData[item.Name];
                                    aggregatedSalesData[item.Name] = (currentData.Price + item.Price, currentData.Quantity + 1);
                                }
                                else
                                {
                                    aggregatedSalesData[item.Name] = (item.Price, 1);
                                }
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"No orders found for {stDate:dd_MM_yy}");
                        return;
                    }
                }
                else
                {
                    Console.WriteLine($"Order file for {stDate:dd_MM_yy} does not exist.");
                }
                stDate = stDate.AddDays(1);
            }

            PrintAggregatedSalesData(aggregatedSalesData);
        }

        private void PrintAggregatedSalesData(Dictionary<string, (double Price, int Quantity)> salesData)
        {
            double totalRevenue = 0;
            Console.WriteLine("\nAggregated Sales Data:");
            string header = string.Format("{0,-25} {1,-10} {2,-10}", "Item", "Quantity", "Total Revenue");
            Console.WriteLine(header);
            Console.WriteLine(new string('-', 50));

            foreach (var item in salesData)
            {
                string line = string.Format("{0,-25} {1,-10} ${2,-10}", item.Key, item.Value.Quantity, item.Value.Price);
                Console.WriteLine(line);
                totalRevenue += item.Value.Price;
            }

            Console.WriteLine(new string('-', 50));
            Console.WriteLine($"Total Revenue: ${totalRevenue}");
            Console.WriteLine(new string('-', 50));
            Console.WriteLine();
        }

    }
}
