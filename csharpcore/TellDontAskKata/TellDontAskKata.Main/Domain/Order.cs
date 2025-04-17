using System.Collections.Generic;
using TellDontAskKata.Main.UseCase;

namespace TellDontAskKata.Main.Domain
{
    public class Order
    {
        public decimal Total { get; set; }
        public string Currency { get; }
        public IList<OrderItem> Items { get; }
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

        public void AddItems(List<OrderItem> orderItems)
        {
            foreach(OrderItem item in orderItems) AddItem(item);
        }
    }
}
