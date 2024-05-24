using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group01RestaurantSystem
{
    //Class representing the menu of the restaurant
    internal class Menu
    {
        //Private field to store the list of menu items
        private List<MenuItem> menuItems;

        //Constructor to initialize the menu with predefined items
        public Menu()
        {
            menuItems = new List<MenuItem>
            {
                new MenuItem(1, "Chicken Alfredo", 12.99, "Fettuccine Alfredo with grilled chicken", "Entree"),
                new MenuItem(2, "Spaghetti", 10.99, "Spaghetti with marinara sauce", "Entree"),
                new MenuItem(3, "Caesar Salad", 8.99, "Romaine lettuce, croutons, parmesan cheese, and Caesar dressing", "Salad"),
                new MenuItem(4, "House Salad", 7.99, "Mixed greens, tomatoes, cucumbers, and choice of dressing", "Salad"),
                new MenuItem(5, "Cheesecake", 5.99, "New York style cheesecake", "Dessert"),
                new MenuItem(6, "Chocolate Cake", 5.99, "Chocolate cake with chocolate frosting", "Dessert"),
                new MenuItem(7, "Coke", 1.99, "Coca-Cola", "Beverage"),
                new MenuItem(8, "Sprite", 1.99, "Sprite", "Beverage"),
                new MenuItem(9, "Sweet Tea", 1.99, "Sweet tea", "Beverage"),
                new MenuItem(10, "Unsweet Tea", 1.99, "Unsweet tea", "Beverage")
            };
        }

        //Method to print the entire menu
        public void PrintMenu()
        {
            //Define column headers and widths
            string header = String.Format("{0,-10} {1,-25} {2,-10} {3}", "Index", "Name", "Price", "Description");
            Console.WriteLine(header);
            Console.WriteLine(new string('-', 100)); //Creates a separator line

            //Iterate through each category and print items in table format
            PrintCategory("Entree");
            PrintCategory("Salad");
            PrintCategory("Dessert");
            PrintCategory("Beverage");
        }

        //Helper method to print items of a specific category
        private void PrintCategory(string category)
        {
            Console.WriteLine($"\n{category}s:");
            foreach (var item in menuItems)
            {
                if (item.Category == category)
                {
                    string line = String.Format("{0,-10} {1,-25} {2,-10} {3}",
                        item.Index,
                        item.Name,
                        item.Price.ToString("C"),
                        item.Description);
                    Console.WriteLine(line);
                }
            }
        }

        //Method to get a menu item by its index
        public MenuItem GetMenuItem(int index)
        {
            if (index >= 0 && index < menuItems.Count)
            {
                return menuItems[index];
            }
            else
            {
                throw new IndexOutOfRangeException("Index out of range.");
            }
        }

        //Property to get the list of menu items
        public List<MenuItem> getMenuItems
        {
            get { return menuItems; }
        }
    }
}
