namespace TellDontAskKata.Main.Domain
{
    public class Product
    {
        public Product(string name, decimal price, Category category)
        {
            Name = name;
            Price = price;
            Category = category;
        }

        public string Name { get; set; }
        public decimal Price { get; set; }
        public Category Category { get; set; }
    }
}