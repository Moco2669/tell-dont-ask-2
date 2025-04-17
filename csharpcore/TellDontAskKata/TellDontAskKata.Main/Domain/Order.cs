using System.Collections.Generic;
using TellDontAskKata.Main.Exceptions;
using TellDontAskKata.Main.Requests;
using TellDontAskKata.Main.UseCase;

namespace TellDontAskKata.Main.Domain
{
    public class Order
    {
        public decimal Total { get; private set; }
        public string Currency { get; }
        public IList<OrderItem> Items { get; }
        public decimal Tax { get; private set; }
        public OrderStatus Status { get; private set; }
        public int Id { get; }

        public Order()
        {
            Total = 0m;
            Currency = "EUR";
            Items = new List<OrderItem>();
            Tax = 0m;
            Status = new Created();
        }

        public Order(int id) : this()
        {
            Id = id;
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

        public void Approve()
        {
            Status = Status.Approve();
        }

        public void Reject()
        {
            Status = Status.Reject();
        }

        public void Ship()
        {
            Status = Status.Ship();
        }

        public bool StatusIs(OrderStatus status)
        {
            return Status.Equals(status);
        }
    }
}
