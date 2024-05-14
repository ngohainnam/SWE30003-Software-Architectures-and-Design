using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group01RestaurantSystem
{
    public class MenuItem
    {
        private double fPrice;
        private string fName;
        private string fDescription;
        private string fCategory;

        public MenuItem(string name, double price, string description, string category)
        {
            fName = name;
            fPrice = price;
            fDescription = description;
            fCategory = category;
        }

        //getters
        public string GetName()
        {
            return fName;
        }
        public double GetPrice()
        {
            return fPrice;
        }
        public string GetDescription()
        {
            return fDescription;
        }
        public string GetCategory()
        {
            return fCategory;
        }
    }
}
