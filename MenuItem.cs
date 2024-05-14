using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group01RestaurantSystem
{
    public class MenuItem
    {
        private int index;
        private double fPrice;
        private string fName;
        private string fDescription;
        private string fCategory;

        public MenuItem(int foodindex, string name, double price, string description, string category)
        {
            index = foodindex;
            fName = name;
            fPrice = price;
            fDescription = description;
            fCategory = category;
        }

        //getters
        public int GetIndex()
        {
            return index;
        }

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
