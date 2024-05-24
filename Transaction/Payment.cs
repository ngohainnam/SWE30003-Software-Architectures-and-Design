using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Abstract class representing a generic payment
public abstract class Payment
{
    //Private field to store the total bill amount
    private double _totalbill;

    //Public property to access and modify the total bill amount
    public double TotalBill
    {
        get
        {
            return _totalbill; //Return the total bill amount
        }
        set
        {
            _totalbill = value; //Set the total bill amount
        }
    }

    //Constructor to initialize the total bill amount
    protected Payment(double totalbill)
    {
        TotalBill = totalbill; //Set the total bill amount
    }

    //Abstract method to process the payment, to be implemented by derived classes
    public abstract bool ProcessPayment();
}
