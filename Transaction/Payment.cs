using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class Payment
{
    private double _totalbill;

    public double TotalBill
    {
        get
        {
            return _totalbill;
        }
        set
        {
            _totalbill = value;
        }
    }

    protected Payment(double totalbill)
    {
        TotalBill = totalbill;
    }

    public abstract bool ProcessPayment();
}
