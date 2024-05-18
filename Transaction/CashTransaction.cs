using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group01RestaurantSystem.Transaction
{
    public class CashTransaction : Payment
    {
        private double _amountPaid;

        public double AmountPaid
        {
            get
            {
                return _amountPaid;
            }
            set
            {
                _amountPaid = value;
            }
        }

        public CashTransaction(double amountPaid, double TotalBill) : base(TotalBill)
        {
            AmountPaid = amountPaid;
        }

        public override bool ProcessPayment()
        {
            Console.WriteLine($"Processing cash payment for amount ${TotalBill}...");

            double change = AmountPaid - TotalBill;

            if ( change > 0 )
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nCash payment processed. Change given: ${change}");
                Console.ForegroundColor = ConsoleColor.White;
                return true;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nNot enough cash to pay for the order...Please take your cash back.");
                Console.ForegroundColor = ConsoleColor.White;
                return false;
            }
        }
    }
}
