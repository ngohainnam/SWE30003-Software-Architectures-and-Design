using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Group01RestaurantSystem
{
    /// <summary>
    /// Enum to represent the status of an order.
    /// </summary>
    internal enum OrderStatus
    {
        Start,      // Order has started
        Preparing,  // Order is being prepared
        Finish      // Order is finished
    }

    /// <summary>
    /// Class to manage the database operations.
    /// </summary>
    internal class Database
    {
        private List<Order> orders;                         // List to store order objects
        private List<OrderQueueEntry> orderQueue;           // List to store order queue entries
        private static Database? instance;                  // Singleton instance of the Database class
        private static readonly object lockObject = new object(); // Object used for thread-safe singleton implementation
        private List<Table> tables;                         // List to store table objects

        /// <summary>
        /// Initializes a new instance of the <see cref="Database"/> class and reads initial data.
        /// </summary>
        public Database()
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            orders = new List<Order>();
            orderQueue = new List<OrderQueueEntry>();
            tables = new List<Table>();
            ReadOrderQueue();                               // Read existing order queue from file
            DateTime now = DateTime.Now;
            string formattedDate = now.ToString("dd_MM_yy");
            ReadOrders(formattedDate);                      // Read today's orders from file
            ReadReservation();                              // Read existing reservations from file
        }

        /// <summary>
        /// Gets the singleton instance of the Database class.
        /// </summary>
        public static Database Instance
        {
            get
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        instance = new Database();          // Create new instance if it doesn't exist
                    }
                    return instance;                        // Return the singleton instance
                }
            }
        }

        /// <summary>
        /// Gets the list of tables.
        /// </summary>
        public List<Table> Tables => tables;

        /// <summary>
        /// Gets the order queue.
        /// </summary>
        public List<OrderQueueEntry> GetOrderQueue()
        {
            return orderQueue;
        }

        /// <summary>
        /// Adds an order to the database.
        /// </summary>
        /// <param name="order">The order to add.</param>
        public void AddOrder(Order order)
        {
            order.CurrentDateTime = DateTime.Now;           // Set the current date and time for the order
            orders.Add(order);                              // Add the order to the list of orders
            orderQueue.Add(new OrderQueueEntry(order, OrderStatus.Start, orderQueue.Count > 0 ? orderQueue.Last().Number + 1 : 1)); // Ternary operation is used for case where orderQueue is empty
            WriteOrderQueue();                              // Write the updated order queue to file
            SaveOrder();                                    // Save the updated orders to file
        }

        /// <summary>
        /// Saves orders to a file.
        /// </summary>
        public void SaveOrder()
        {
            if (!Directory.Exists("Orders"))
            {
                Directory.CreateDirectory("Orders");        // Create "Orders" directory if it doesn't exist
            }
            DateTime now = DateTime.Now;
            string formattedDate = now.ToString("dd_MM_yy");
            string fileName = $"Orders/orders_{formattedDate}.json"; // Generate file name based on current date
            string jsonString = JsonSerializer.Serialize(orders, new JsonSerializerOptions { WriteIndented = true }); // Serialize the orders list to JSON
            File.WriteAllText(fileName, jsonString);        // Write the JSON string to the file
        }

        /// <summary>
        /// Reads orders from a file.
        /// </summary>
        /// <param name="date">The date to read orders for.</param>
        public void ReadOrders(string date)
        {
            string fileName = $"Orders/orders_{date}.json"; // Generate file name based on given date
            if (File.Exists(fileName))
            {
                string jsonString = File.ReadAllText(fileName); // Read the JSON string from the file
                var deserializedOrders = JsonSerializer.Deserialize<List<Order>>(jsonString); // Deserialize the JSON string to a list of orders
                if (deserializedOrders != null)
                {
                    orders = deserializedOrders;            // Set the orders list to the deserialized data
                }
                else
                {
                    Console.WriteLine($"No orders found for {date}");
                }
            }
        }

        /// <summary>
        /// Updates the status of an order.
        /// </summary>
        /// <param name="order">The order to update.</param>
        public void UpdateOrderStatus(Order order)
        {
            var entry = orderQueue.FirstOrDefault(e => e.Order == order); // Find the order queue entry for the given order
            if (entry != null)
            {
                switch (entry.Status)
                {
                    case OrderStatus.Start:
                        entry.Status = OrderStatus.Preparing; // Update status to "Preparing"
                        break;
                    case OrderStatus.Preparing:
                        entry.Status = OrderStatus.Finish;   // Update status to "Finish"
                        break;
                    case OrderStatus.Finish:
                        orderQueue.Remove(entry);            // Remove the entry from the order queue
                        break;
                }
                WriteOrderQueue();                           // Write the updated order queue to file
            }
        }

        /// <summary>
        /// Reads the order queue from a file.
        /// </summary>
        public void ReadOrderQueue()
        {
            string fileName = $"OrderQueue/OrderQueue.json"; // Define the file name
            if (File.Exists(fileName))
            {
                string jsonString = File.ReadAllText(fileName); // Read the JSON string from the file
                var deserializedData = JsonSerializer.Deserialize<List<OrderQueueEntry>>(jsonString); // Deserialize the JSON string to a list of order queue entries
                if (deserializedData != null)
                {
                    orderQueue = deserializedData;           // Set the order queue to the deserialized data
                }
            }
        }

        /// <summary>
        /// Writes the order queue to a file.
        /// </summary>
        public void WriteOrderQueue()
        {
            if (!Directory.Exists("OrderQueue"))
            {
                Directory.CreateDirectory("OrderQueue");    // Create "OrderQueue" directory if it doesn't exist
            }
            string fileName = $"OrderQueue/OrderQueue.json"; // Define the file name
            string jsonString = JsonSerializer.Serialize(orderQueue, new JsonSerializerOptions { WriteIndented = true }); // Serialize the order queue to JSON
            File.WriteAllText(fileName, jsonString);        // Write the JSON string to the file
        }

        /// <summary>
        /// Saves reservations to a file.
        /// </summary>
        public void SaveReservation()
        {
            string reservationFilePath = "reservation_Data.json"; // Define the file path
            string updatedReservationJson = JsonSerializer.Serialize(tables, new JsonSerializerOptions { WriteIndented = true }); // Serialize the tables list to JSON
            File.WriteAllText(reservationFilePath, updatedReservationJson); // Write the JSON string to the file
        }

        /// <summary>
        /// Reads reservations from a file.
        /// </summary>
        public void ReadReservation()
        {
            string reservationFilePath = "reservation_Data.json"; // Define the file path
            if (File.Exists(reservationFilePath))
            {
                string reservationJson = File.ReadAllText(reservationFilePath); // Read the JSON string from the file

                if (!string.IsNullOrWhiteSpace(reservationJson))
                {
                    try
                    {
                        var deserializedReservation = JsonSerializer.Deserialize<List<Table>>(reservationJson); // Deserialize the JSON string to a list of tables
                        if (deserializedReservation != null && deserializedReservation.Count > 0)
                        {
                            tables = deserializedReservation; // Set the tables list to the deserialized data
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

        /// <summary>
        /// Reads sales data within a date range.
        /// </summary>
        /// <param name="startDate">The start date of the range.</param>
        /// <param name="endDate">The end date of the range.</param>
        public void ReadSalesData(DateTime startDate, DateTime endDate)
        {
            DateTime stDate = startDate;
            DateTime etDate = endDate;

            Dictionary<string, (double Price, int Quantity)> aggregatedSalesData = new Dictionary<string, (double, int)>(); // Dictionary to store aggregated sales data

            while (stDate <= etDate)
            {
                string fileName = $"Orders/orders_{stDate:dd_MM_yy}.json"; // Generate file name based on the current date in the loop
                if (File.Exists(fileName))
                {
                    string jsonString = File.ReadAllText(fileName); // Read the JSON string from the file
                    var deserializedOrders = JsonSerializer.Deserialize<List<Order>>(jsonString); // Deserialize the JSON string to a list of orders
                    if (deserializedOrders != null)
                    {
                        foreach (var order in deserializedOrders)
                        {
                            foreach (var item in order.OrderItems)
                            {
                                if (aggregatedSalesData.ContainsKey(item.Name))
                                {
                                    var currentData = aggregatedSalesData[item.Name];
                                    aggregatedSalesData[item.Name] = (currentData.Price + item.Price, currentData.Quantity + 1); // Update the aggregated data
                                }
                                else
                                {
                                    aggregatedSalesData[item.Name] = (item.Price, 1); // Add new entry to the aggregated data
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
                stDate = stDate.AddDays(1); // Move to the next date
            }

            PrintAggregatedSalesData(aggregatedSalesData); // Print the aggregated sales data
        }

        /// <summary>
        /// Prints aggregated sales data.
        /// </summary>
        /// <param name="salesData">The sales data to print.</param>
        private void PrintAggregatedSalesData(Dictionary<string, (double Price, int Quantity)> salesData)
        {
            double totalRevenue = 0;
            Console.WriteLine("\nAggregated Sales Data:");
            Console.WriteLine(new string('-', 50));
            string header = string.Format("{0,-25} {1,-10} {2,-10}", "Item", "Quantity", "Total Revenue");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(header);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(new string('-', 50));

            foreach (var item in salesData)
            {
                string line = string.Format("{0,-25} {1,-10} ${2,-10}", item.Key, item.Value.Quantity, Math.Round(item.Value.Price, 2)); // Format the line for each item
                Console.WriteLine(line);
                totalRevenue += item.Value.Price; // Add to the total revenue
            }

            Console.WriteLine(new string('-', 50));
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Total Revenue: ${Math.Round(totalRevenue, 2)}"); // Print the total revenue
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(new string('-', 50));
            Console.WriteLine();
        }

        /// <summary>
        /// Writes menu items to a file.
        /// </summary>
        /// <param name="menu">The menu items to write.</param>
        public void WriteMenuItems(List<MenuItem> menu)
        {
            string menuFilePath = "Menu/Menu.json"; // Define the file path
            string updatedMenuJson = JsonSerializer.Serialize(menu, new JsonSerializerOptions { WriteIndented = true }); // Serialize the menu list to JSON
            File.WriteAllText(menuFilePath, updatedMenuJson); // Write the JSON string to the file
        }

        /// <summary>
        /// Reads menu items from a file.
        /// </summary>
        /// <returns>The list of menu items.</returns>
        public List<MenuItem> ReadMenuItems()
        {
            string menuFilePath = "Menu/Menu.json"; // Define the file path
            List<MenuItem> returnMenu = new List<MenuItem>();
            if (File.Exists(menuFilePath))
            {
                string jsonString = File.ReadAllText(menuFilePath); // Read the JSON string from the file
                var deserializedMenuItems = JsonSerializer.Deserialize<List<MenuItem>>(jsonString); // Deserialize the JSON string to a list of menu items
                if (deserializedMenuItems != null)
                {
                    returnMenu = deserializedMenuItems; // Set the menu list to the deserialized data
                }
                else
                {
                    Console.WriteLine($"No menu items found for {menuFilePath}");
                }
            }
            else
            {
                Console.WriteLine("Menu file does not exist.");
            }
            return returnMenu;
        }
    }
}
