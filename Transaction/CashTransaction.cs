using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group01RestaurantSystem.Transaction
{
    //Class to handle cash transactions
    public class CashTransaction : Payment
    {
        //Private field to store the amount paid
        private double _amountPaid;

        //Public property to access and modify the amount paid
        public double AmountPaid
        {
            get
            {
                return _amountPaid; //Return the amount paid
            }
            set
            {
                _amountPaid = value; //Set the amount paid
            }
        }

        //Constructor to initialize the amount paid and the total bill amount
        public CashTransaction(double amountPaid, double TotalBill) : base(TotalBill)
        {
            AmountPaid = amountPaid; //Set the amount paid
        }

        //Method to process the cash payment
        public override bool ProcessPayment()
        {
            Console.WriteLine($"Processing cash payment for amount ${Math.Round(TotalBill, 2)}...");

            //Calculate the change to be given back
            double change = AmountPaid - TotalBill;

            //Check if the amount paid is sufficient to cover the total bill
            if (change > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green; 
                Console.WriteLine($"\nCash payment processed. Change given: ${Math.Round(change, 2)}");
                Console.ForegroundColor = ConsoleColor.White; 
                return true; //Payment successful
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red; 
                Console.WriteLine("\nNot enough cash to pay for the order...Please take your cash back.");
                Console.ForegroundColor = ConsoleColor.White; 
                return false; //Payment failed
            }
        }
    }
}
