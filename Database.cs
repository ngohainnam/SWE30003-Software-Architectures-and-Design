using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Group01RestaurantSystem
{
    //Enum to represent the status of an order
    enum OrderStatus
    {
        Start,      //Order has started
        Preparing,  //Order is being prepared
        Finish      //Order is finished
    }

    //Class to manage the database operations
    internal class Database
    {
        //Private fields to store orders, order queue, and tables
        private List<Order> orders;                         //List to store order objects
        private List<OrderQueueEntry> orderQueue;           //List to store order queue entries
        private static Database? instance;                  //Singleton instance of the Database class
        private static readonly object lockObject = new object(); //Object used for thread-safe singleton implementation
        private List<Table> tables;                         //List to store table objects

        //Constructor to initialize the database and read initial data
        public Database()
        {
            orders = new List<Order>();
            orderQueue = new List<OrderQueueEntry>();
            tables = new List<Table>();
            ReadOrderQueue();                               //Read existing order queue from file
            DateTime now = DateTime.Now;
            string formattedDate = now.ToString("dd_MM_yy");
            ReadOrders(formattedDate);                      //Read today's orders from file
            ReadReservation();                              //Read existing reservations from file
        }

        //Singleton pattern to ensure only one instance of Database exists
        public static Database Instance
        {
            get
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        instance = new Database();          //Create new instance if it doesn't exist
                    }
                    return instance;                        //Return the singleton instance
                }
            }
        }

        //Property to access the list of tables
        public List<Table> Tables => tables;

        //Method to get the order queue
        public List<OrderQueueEntry> GetOrderQueue()
        {
            return orderQueue;
        }

        //Method to add an order to the database
        public void AddOrder(Order order)
        {
            order.CurrentDateTime = DateTime.Now;           //Set the current date and time for the order
            orders.Add(order);                              //Add the order to the list of orders
            orderQueue.Add(new OrderQueueEntry(order, OrderStatus.Start)); //Add the order to the order queue with status "Start"
            WriteOrderQueue();                              //Write the updated order queue to file
            SaveOrder();                                    //Save the updated orders to file
        }

        //Method to save orders to a file
        public void SaveOrder()
        {
            if (!Directory.Exists("Orders"))
            {
                Directory.CreateDirectory("Orders");        //Create "Orders" directory if it doesn't exist
            }
            DateTime now = DateTime.Now;
            string formattedDate = now.ToString("dd_MM_yy");
            string fileName = $"Orders/orders_{formattedDate}.json"; //Generate file name based on current date
            string jsonString = JsonSerializer.Serialize(orders, new JsonSerializerOptions { WriteIndented = true }); //Serialize the orders list to JSON
            File.WriteAllText(fileName, jsonString);        //Write the JSON string to the file
            Console.WriteLine($"Data has been written to {fileName}");
        }

        //Method to read orders from a file
        public void ReadOrders(string date)
        {
            string fileName = $"Orders/orders_{date}.json"; //Generate file name based on given date
            if (File.Exists(fileName))
            {
                string jsonString = File.ReadAllText(fileName); //Read the JSON string from the file
                var deserializedOrders = JsonSerializer.Deserialize<List<Order>>(jsonString); //Deserialize the JSON string to a list of orders
                if (deserializedOrders != null)
                {
                    orders = deserializedOrders;            //Set the orders list to the deserialized data
                }
                else
                {
                    Console.WriteLine($"No orders found for {date}");
                }
            }
        }

        //Method to update the status of an order
        public void UpdateOrderStatus(Order order)
        {
            var entry = orderQueue.FirstOrDefault(e => e.Order == order); //Find the order queue entry for the given order
            if (entry != null)
            {
                switch (entry.Status)
                {
                    case OrderStatus.Start:
                        entry.Status = OrderStatus.Preparing; //Update status to "Preparing"
                        break;
                    case OrderStatus.Preparing:
                        entry.Status = OrderStatus.Finish;   //Update status to "Finish"
                        break;
                    case OrderStatus.Finish:
                        orderQueue.Remove(entry);            //Remove the entry from the order queue
                        break;
                }
                WriteOrderQueue();                           //Write the updated order queue to file
            }
        }

        //Method to read the order queue from a file
        public void ReadOrderQueue()
        {
            string fileName = $"OrderQueue/OrderQueue.json"; //Define the file name
            if (File.Exists(fileName))
            {
                string jsonString = File.ReadAllText(fileName); //Read the JSON string from the file
                var deserializedData = JsonSerializer.Deserialize<List<OrderQueueEntry>>(jsonString); //Deserialize the JSON string to a list of order queue entries
                if (deserializedData != null)
                {
                    orderQueue = deserializedData;           //Set the order queue to the deserialized data
                }
            }
        }

        //Method to write the order queue to a file
        public void WriteOrderQueue()
        {
            if (!Directory.Exists("OrderQueue"))
            {
                Directory.CreateDirectory("OrderQueue");    //Create "OrderQueue" directory if it doesn't exist
            }
            string fileName = $"OrderQueue/OrderQueue.json"; //Define the file name
            string jsonString = JsonSerializer.Serialize(orderQueue, new JsonSerializerOptions { WriteIndented = true }); //Serialize the order queue to JSON
            File.WriteAllText(fileName, jsonString);        //Write the JSON string to the file
        }

        //Method to save reservations to a file
        public void SaveReservation()
        {
            string reservationFilePath = "reservation_Data.json"; //Define the file path
            string updatedReservationJson = JsonSerializer.Serialize(tables, new JsonSerializerOptions { WriteIndented = true }); //Serialize the tables list to JSON
            File.WriteAllText(reservationFilePath, updatedReservationJson); //Write the JSON string to the file
        }

        //Method to read reservations from a file
        public void ReadReservation()
        {
            string reservationFilePath = "reservation_Data.json"; //Define the file path
            if (File.Exists(reservationFilePath))
            {
                string reservationJson = File.ReadAllText(reservationFilePath); //Read the JSON string from the file

                if (!string.IsNullOrWhiteSpace(reservationJson))
                {
                    try
                    {
                        var deserializedReservation = JsonSerializer.Deserialize<List<Table>>(reservationJson); //Deserialize the JSON string to a list of tables
                        if (deserializedReservation != null && deserializedReservation.Count > 0)
                        {
                            tables = deserializedReservation; //Set the tables list to the deserialized data
                        }
                        else
                        {
                            Console.WriteLine("reservation_Data.json is empty. Initialized an empty reservation list.");
                        }
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"Failed to deserialize reservation data: {ex.Message}");
                        tables = new List<Table>(); //Fallback to an empty list
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

        //Method to read sales data within a date range
        public void ReadSalesData(DateTime startDate, DateTime endDate)
        {
            DateTime stDate = startDate;
            DateTime etDate = endDate;

            Dictionary<string, (double Price, int Quantity)> aggregatedSalesData = new Dictionary<string, (double, int)>(); //Dictionary to store aggregated sales data

            while (stDate <= etDate)
            {
                string fileName = $"Orders/orders_{stDate:dd_MM_yy}.json"; //Generate file name based on the current date in the loop
                if (File.Exists(fileName))
                {
                    string jsonString = File.ReadAllText(fileName); //Read the JSON string from the file
                    var deserializedOrders = JsonSerializer.Deserialize<List<Order>>(jsonString); //Deserialize the JSON string to a list of orders
                    if (deserializedOrders != null)
                    {
                        foreach (var order in deserializedOrders)
                        {
                            foreach (var item in order.OrderItems)
                            {
                                if (aggregatedSalesData.ContainsKey(item.Name))
                                {
                                    var currentData = aggregatedSalesData[item.Name];
                                    aggregatedSalesData[item.Name] = (currentData.Price + item.Price, currentData.Quantity + 1); //Update the aggregated data
                                }
                                else
                                {
                                    aggregatedSalesData[item.Name] = (item.Price, 1); //Add new entry to the aggregated data
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
                stDate = stDate.AddDays(1); //Move to the next date
            }

            PrintAggregatedSalesData(aggregatedSalesData); //Print the aggregated sales data
        }

        //Method to print aggregated sales data
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
                string line = string.Format("{0,-25} {1,-10} ${2,-10}", item.Key, item.Value.Quantity, item.Value.Price); //Format the line for each item
                Console.WriteLine(line);
                totalRevenue += item.Value.Price; //Add to the total revenue
            }

            Console.WriteLine(new string('-', 50));
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Total Revenue: ${totalRevenue}"); //Print the total revenue
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(new string('-', 50));
            Console.WriteLine();
        }
    }
}
