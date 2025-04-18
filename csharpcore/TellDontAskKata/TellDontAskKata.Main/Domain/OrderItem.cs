namespace TellDontAskKata.Main.Domain
{
    public class OrderItem
    {
        public Product Product { get; }
        public int Quantity { get; }
        public decimal TaxedAmount { get; }
        public decimal Tax { get; }

        public OrderItem(Product product, int quantity)
        {
            this.Product = product;
            this.Quantity = quantity;
            
            var unitaryTax = Round((product.Price / 100m) * product.Category.TaxPercentage);
            var unitaryTaxedAmount = Round(product.Price + unitaryTax);
            
            Tax = Round(unitaryTax * quantity);
            TaxedAmount = Round(unitaryTaxedAmount * quantity);
        }
        
        private static decimal Round(decimal amount)
        {
            return decimal.Round(amount, 2, System.MidpointRounding.ToPositiveInfinity);
        }
    }
}
