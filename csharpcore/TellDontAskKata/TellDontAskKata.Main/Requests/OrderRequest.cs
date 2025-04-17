using TellDontAskKata.Main.Domain;

namespace TellDontAskKata.Main.Requests
{
    public abstract class OrderRequest
    {
        public int OrderId { get; }
        
        protected OrderRequest(int orderId)
        {
            OrderId = orderId;
        }

        public abstract void ExecuteOn(Order order);
    }
}
