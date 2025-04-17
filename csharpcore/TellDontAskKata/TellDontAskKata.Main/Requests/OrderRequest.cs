using TellDontAskKata.Main.Domain;

namespace TellDontAskKata.Main.Requests
{
    public abstract class OrderRequest
    {
        public int OrderId { get; }
        
        public OrderRequest(int orderId)
        {
            OrderId = orderId;
        }

        public abstract void ExecuteRequest(Order order);
    }
}
