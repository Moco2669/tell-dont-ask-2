using TellDontAskKata.Main.Domain;

namespace TellDontAskKata.Main.Requests
{
    public class ShipRequest : OrderRequest
    {
        public ShipRequest(int orderId) : base(orderId)
        {
        }

        public override void ExecuteOn(Order order)
        {
            order.Ship();
        }
    }
}