namespace Group01RestaurantSystem
{
    //Class for Item in the Menu
    public class MenuItem
    {
        //Define every variable needed
        public int Index { get; set; }
        public double Price { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }

        //Making a constructor
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
