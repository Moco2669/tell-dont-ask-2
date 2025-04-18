using TellDontAskKata.Main.Domain;

namespace TellDontAskKata.Main.Requests
{
    public class ApproveRequest : OrderRequest
    {
        public ApproveRequest(int orderId) : base(orderId)
        {
        }

        public override void ExecuteOn(Order order)
        {
            order.Approve();
        }
    }
}