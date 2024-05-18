using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group01RestaurantSystem.Transaction
{
    public class CardTransaction : Payment
    {
        public string CardNumber { get; private set; }

        public CardTransaction(string cardNumber, double TotalBill) : base(TotalBill)
        {
            CardNumber = cardNumber;
        }

        public override bool ProcessPayment()
        {
            Console.WriteLine($"Processing card payment for amount ${TotalBill}...");

            Random random = new Random();
            double cardamount = random.Next(0, 100);
            
            if ( cardamount > TotalBill) 
            {
                Console.WriteLine($"Card amount: ${cardamount}");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nCard payment processed.");
                Console.ForegroundColor = ConsoleColor.White;
                return true;
            }
            else
            {
                Console.WriteLine($"Card amount: ${cardamount}");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nCard payment failed.");
                Console.ForegroundColor = ConsoleColor.White;
                return false;
            }
        }
    }
}
