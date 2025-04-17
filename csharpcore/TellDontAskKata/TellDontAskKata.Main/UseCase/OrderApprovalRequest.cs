namespace TellDontAskKata.Main.UseCase
{
    public class OrderApprovalRequest
    {
        public OrderApprovalRequest(int orderId, bool approved)
        {
            OrderId = orderId;
            Approved = approved;
        }

        public int OrderId { get; set; }
        public bool Approved { get; set; }
    }
}
