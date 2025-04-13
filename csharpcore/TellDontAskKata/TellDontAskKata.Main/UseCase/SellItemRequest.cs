namespace TellDontAskKata.Main.UseCase
{
    public class SellItemRequest
    {
        public SellItemRequest()
        {
        }

        public SellItemRequest(string productName, int quantity)
        {
            ProductName = productName;
            Quantity = quantity;
        }

        public SellItemRequest(string productName) : this()
        {
            ProductName = productName;
        }

        public int Quantity { get; set; }
        public string ProductName { get; set; }
    }
}
