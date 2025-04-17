namespace TellDontAskKata.Main.UseCase
{
    public class OrderShipmentRequest
    {
        public OrderShipmentRequest()
        {
        }

        public OrderShipmentRequest(int orderId)
        {
            OrderId = orderId;
        }

        public int OrderId { get; }
    }
}
