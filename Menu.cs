using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group01RestaurantSystem
{
    internal class Menu
    {
        private MenuItem[] menuItems = new MenuItem[10];

        public Menu()
        {
            menuItems[0] = new MenuItem(1, "Chicken Alfredo", 12.99, "Fettuccine Alfredo with grilled chicken", "Entree");
            menuItems[1] = new MenuItem(2, "Spaghetti", 10.99, "Spaghetti with marinara sauce", "Entree");
            menuItems[2] = new MenuItem(3, "Caesar Salad", 8.99, "Romaine lettuce, croutons, parmesan cheese, and Caesar dressing", "Salad");
            menuItems[3] = new MenuItem(4, "House Salad", 7.99, "Mixed greens, tomatoes, cucumbers, and choice of dressing", "Salad");
            menuItems[4] = new MenuItem(5, "Cheesecake", 5.99, "New York style cheesecake", "Dessert");
            menuItems[5] = new MenuItem(6, "Chocolate Cake", 5.99, "Chocolate cake with chocolate frosting", "Dessert");
            menuItems[6] = new MenuItem(7, "Coke", 1.99, "Coca-Cola", "Beverage");
            menuItems[7] = new MenuItem(8, "Sprite", 1.99, "Sprite", "Beverage");
            menuItems[8] = new MenuItem(9, "Sweet Tea", 1.99, "Sweet tea", "Beverage");
            menuItems[9] = new MenuItem(10, "Unsweet Tea", 1.99, "Unsweet tea", "Beverage");
        }
        // print menu
        public void PrintMenu()
        {
            // Define column headers and widths
            string header = String.Format("{0,-10} {1,-25} {2,-10} {3}", "Index", "Name", "Price", "Description");
            Console.WriteLine(header);
            Console.WriteLine(new string('-', 100)); // Creates a separator line

            // Iterate through each category and print items in table format
            PrintCategory("Entree");
            PrintCategory("Salad");
            PrintCategory("Dessert");
            PrintCategory("Beverage");
        }

        private void PrintCategory(string category)
        {
            Console.WriteLine($"\n{category}s:");
            foreach (var item in menuItems)
            {
                if (item.GetCategory == category)
                {
                    string line = String.Format("{0,-10} {1,-25} {2,-10} {3}",
                        item.GetIndex,
                        item.GetName,
                        item.GetPrice.ToString("C"),
                        item.GetDescription);
                    Console.WriteLine(line);
                }
            }
        }

        public MenuItem GetMenuItem(int index)
        {
            if (index >= 0 && index < menuItems.Length)
            {
                return menuItems[index];
            }
            else
            {
                throw new IndexOutOfRangeException("Index out of range.");
            }
        }

        public MenuItem[] getMenuItems
        {
            get { return menuItems; }
        }
    }
}
