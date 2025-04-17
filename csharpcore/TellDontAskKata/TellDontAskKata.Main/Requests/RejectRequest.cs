using TellDontAskKata.Main.Domain;

namespace TellDontAskKata.Main.Requests
{
    public class RejectRequest : OrderRequest
    {
        public RejectRequest(int orderId) : base(orderId)
        {
        }

        public override void ExecuteRequest(Order order)
        {
            order.Reject();
        }
    }
}