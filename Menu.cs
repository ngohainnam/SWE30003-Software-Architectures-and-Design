using System;
using System.Collections.Generic;

namespace Group01RestaurantSystem
{
    /// <summary>
    /// Represents the menu of the restaurant.
    /// </summary>
    internal class Menu
    {
        /// <summary>
        /// Private field to store the list of menu items.
        /// </summary>
        private List<MenuItem> menuItems;

        /// <summary>
        /// Initializes a new instance of the <see cref="Menu"/> class with predefined items.
        /// </summary>
        public Menu()
        {
            menuItems = Database.Instance.ReadMenuItems();
        }

        /// <summary>
        /// Prints the entire menu.
        /// </summary>
        public void PrintMenu()
        {
            // Define column headers and widths
            string header = string.Format("{0,-10} {1,-25} {2,-10} {3}", "Index", "Name", "Price", "Description");
            Console.WriteLine(header);
            Console.WriteLine(new string('-', 100)); // Creates a separator line

            // Iterate through each category and print items in table format
            PrintCategory("Entree");
            PrintCategory("Salad");
            PrintCategory("Dessert");
            PrintCategory("Beverage");
        }

        /// <summary>
        /// Helper method to print items of a specific category.
        /// </summary>
        /// <param name="category">The category to print.</param>
        private void PrintCategory(string category)
        {
            Console.WriteLine($"\n{category}s:");
            foreach (var item in menuItems)
            {
                if (item.Category == category)
                {
                    string line = string.Format("{0,-10} {1,-25} {2,-10} {3}",
                        item.Index,
                        item.Name,
                        item.Price.ToString("C"),
                        item.Description);
                    Console.WriteLine(line);
                }
            }
        }

        /// <summary>
        /// Gets a menu item by its index.
        /// </summary>
        /// <param name="index">The index of the menu item.</param>
        /// <returns>The menu item.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown when the index is out of range.</exception>
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

        /// <summary>
        /// Gets or sets the list of menu items.
        /// </summary>
        public List<MenuItem> MenuItems
        {
            get { return menuItems; }
            set { menuItems = value; }
        }
    }
}
