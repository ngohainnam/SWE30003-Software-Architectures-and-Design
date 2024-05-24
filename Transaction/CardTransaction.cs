using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Group01RestaurantSystem.Transaction
{
    public class CardTransaction : Payment
    {
        // Property to store the card number in the format XXXX-YYYY-ZZZZ-MMMM
        public string CardNumber { get; private set; }

        // Constructor to initialize the card number and the total bill amount
        public CardTransaction(string cardNumber, double TotalBill) : base(TotalBill)
        {
            if (IsValidCardNumber(cardNumber))
            {
                CardNumber = cardNumber; // Set the card number if valid
            }
            else
            {
                throw new ArgumentException("Invalid card number format. Expected format is XXXX-YYYY-ZZZZ-MMMM.");
            }
        }

        // Method to process the card payment
        public override bool ProcessPayment()
        {
            Console.WriteLine($"Processing card payment for amount ${Math.Round(TotalBill, 2)}...");

            // Generate a random card amount for the payment simulation
            Random random = new Random();
            double cardamount = random.Next(0, 100);

            // Check if the card amount is greater than the total bill
            if (cardamount > TotalBill)
            {
                Console.WriteLine($"Card amount: ${Math.Round(cardamount, 2)}");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nCard payment processed.");
                Console.ForegroundColor = ConsoleColor.White;
                return true; // Payment successful
            }
            else
            {
                Console.WriteLine($"Card amount: ${Math.Round(cardamount, 2)}");
                Console.ForegroundColor = ConsoleColor.Red; 
                Console.WriteLine("\nCard payment failed.");
                Console.ForegroundColor = ConsoleColor.White;
                return false; // Payment failed
            }
        }

        // Method to validate the card number format
        private bool IsValidCardNumber(string cardNumber)
        {
            // Regular expression to match the format XXXX-YYYY-ZZZZ-MMMM
            string pattern = @"^\d{4}-\d{4}-\d{4}-\d{4}$";
            return Regex.IsMatch(cardNumber, pattern);
        }
    }
}
