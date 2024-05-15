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
        public int GetIndex
        {
            get => index;
        }

        public string GetName
        {
            get=> fName;
        }
        public double GetPrice
        {
            get=> fPrice;
        }
        public string GetDescription
        {
            get => fDescription;
        }
        public string GetCategory
        {
            get=> fCategory;
        }
    }
}
