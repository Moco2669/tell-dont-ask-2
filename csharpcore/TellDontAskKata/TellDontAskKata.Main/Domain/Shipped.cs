using TellDontAskKata.Main.Exceptions;

namespace TellDontAskKata.Main.Domain
{
    public class Shipped : OrderStatus
    {
        public override OrderStatus Approve()
        {
            throw new ShippedOrdersCannotBeChangedException();
        }

        public override OrderStatus Reject()
        {
            throw new ShippedOrdersCannotBeChangedException();
        }

        public override OrderStatus Ship()
        {
            throw new OrderCannotBeShippedTwiceException();
        }
    }
}