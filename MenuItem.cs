namespace Group01RestaurantSystem
{
    /// <summary>
    /// Represents an item in the restaurant menu.
    /// </summary>
    public class MenuItem
    {
        /// <summary>
        /// Gets or sets the index of the menu item.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Gets or sets the price of the menu item.
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Gets or sets the name of the menu item.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the menu item.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the category of the menu item.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuItem"/> class.
        /// </summary>
        /// <param name="index">The index of the menu item.</param>
        /// <param name="name">The name of the menu item.</param>
        /// <param name="price">The price of the menu item.</param>
        /// <param name="description">The description of the menu item.</param>
        /// <param name="category">The category of the menu item.</param>
        public MenuItem(int index, string name, double price, string description, string category)
        {
            Index = index;
            Name = name;
            Price = price;
            Description = description;
            Category = category;
        }
    }
}
