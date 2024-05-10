using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group01RestaurantSystem
{
    internal class Menu
    {
        MenuItem[] menuItems = new MenuItem[10];

        public Menu()
        {
            menuItems[0] = new MenuItem("Chicken Alfredo", 12.99, "Fettuccine Alfredo with grilled chicken", "Entree");
            menuItems[1] = new MenuItem("Spaghetti", 10.99, "Spaghetti with marinara sauce", "Entree");
            menuItems[2] = new MenuItem("Caesar Salad", 8.99, "Romaine lettuce, croutons, parmesan cheese, and Caesar dressing", "Salad");
            menuItems[3] = new MenuItem("House Salad", 7.99, "Mixed greens, tomatoes, cucumbers, and choice of dressing", "Salad");
            menuItems[4] = new MenuItem("Cheesecake", 5.99, "New York style cheesecake", "Dessert");
            menuItems[5] = new MenuItem("Chocolate Cake", 5.99, "Chocolate cake with chocolate frosting", "Dessert");
            menuItems[6] = new MenuItem("Coke", 1.99, "Coca-Cola", "Beverage");
            menuItems[7] = new MenuItem("Sprite", 1.99, "Sprite", "Beverage");
            menuItems[8] = new MenuItem("Sweet Tea", 1.99, "Sweet tea", "Beverage");
            menuItems[9] = new MenuItem("Unsweet Tea", 1.99, "Unsweet tea", "Beverage");
        }
        // print menu
        public void PrintMenu()
        {
            Console.WriteLine("Menu:");
            Console.WriteLine("the Entree can be:");
            for (int i = 0; i < menuItems.Length; i++)
            {
                if (menuItems[i].GetCategory() == "Entree")
                {
                    Console.WriteLine(menuItems[i].GetName() + " - " + menuItems[i].GetPrice().ToString("C") + " - " + menuItems[i].GetDescription());
                }
            }
            Console.WriteLine();
            Console.WriteLine("the Salad can be:");
            for (int i = 0; i < menuItems.Length; i++)
            {
                if (menuItems[i].GetCategory() == "Salad")
                {
                    Console.WriteLine(menuItems[i].GetName() + " - " + menuItems[i].GetPrice().ToString("C") + " - " + menuItems[i].GetDescription());
                }
            }
            Console.WriteLine();
            Console.WriteLine("the Dessert can be:");
            for (int i = 0; i < menuItems.Length; i++)
            {
                if (menuItems[i].GetCategory() == "Dessert")
                {
                    Console.WriteLine(menuItems[i].GetName() + " - " + menuItems[i].GetPrice().ToString("C") + " - " + menuItems[i].GetDescription());
                }
            }
            Console.WriteLine();
            Console.WriteLine("the Beverage can be:");
            for (int i = 0; i < menuItems.Length; i++)
            {
                if (menuItems[i].GetCategory() == "Beverage")
                {
                    Console.WriteLine(menuItems[i].GetName() + " - " + menuItems[i].GetPrice().ToString("C") + " - " + menuItems[i].GetDescription());
                }
            }
            Console.WriteLine();
        }
    }
}
