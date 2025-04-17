using System.Collections.Generic;
using TellDontAskKata.Main.UseCase;

namespace TellDontAskKata.Main.Domain
{
    public class Order
    {
        public decimal Total { get; private set; }
        public string Currency { get; }
        public IList<OrderItem> Items { get; }
        public decimal Tax { get; private set; }
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

        public void ExecuteRequest(OrderApprovalRequest request)
        {
            if (request.Approved) Approve();
            else Reject();
        }

        private void Approve()
        {
            if (Status == OrderStatus.Shipped) throw new ShippedOrdersCannotBeChangedException();
            if (Status == OrderStatus.Rejected) throw new RejectedOrderCannotBeApprovedException();
            Status = OrderStatus.Approved;
        }

        private void Reject()
        {
            if (Status == OrderStatus.Shipped) throw new ShippedOrdersCannotBeChangedException();
            if (Status == OrderStatus.Approved) throw new ApprovedOrderCannotBeRejectedException();
            Status = OrderStatus.Rejected;
        }

        public void Ship()
        {
            if (Status == OrderStatus.Shipped) throw new OrderCannotBeShippedTwiceException();
            if (Status == OrderStatus.Rejected || Status == OrderStatus.Created) throw new OrderCannotBeShippedException();
            Status = OrderStatus.Shipped;
        }
    }
}
