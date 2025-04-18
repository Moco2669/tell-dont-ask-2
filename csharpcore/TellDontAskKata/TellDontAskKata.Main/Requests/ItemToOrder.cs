namespace TellDontAskKata.Main.Requests
{
    public class ItemToOrder
    {
        public ItemToOrder()
        {
        }

        public ItemToOrder(string productName, int quantity)
        {
            ProductName = productName;
            Quantity = quantity;
        }

        public ItemToOrder(string productName) : this()
        {
            ProductName = productName;
        }

        public int Quantity { get; }
        public string ProductName { get; }
    }
}
