namespace Group01RestaurantSystem
{
    public class MenuItem
    {
        public int Index { get; set; }
        public double Price { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }


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
