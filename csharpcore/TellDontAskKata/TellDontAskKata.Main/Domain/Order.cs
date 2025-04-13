using System.Collections.Generic;

namespace TellDontAskKata.Main.Domain
{
    public class Order
    {
        public decimal Total { get; set; }
        public string Currency { get; set; }
        public IList<OrderItem> Items { get; set; }
        public decimal Tax { get; set; }
        public OrderStatus Status { get; set; }
        public int Id { get; set; }

        public Order()
        {
            Total = 0m;
            Currency = "EUR";
            Items = new List<OrderItem>();
            Tax = 0m;
            Status = OrderStatus.Created;
        }

        public void AddItem(OrderItem orderItem)
        {
            Items.Add(orderItem);
            Tax += orderItem.Tax;
            Total += orderItem.TaxedAmount;
        }
    }
}
