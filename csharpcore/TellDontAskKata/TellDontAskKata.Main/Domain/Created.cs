using TellDontAskKata.Main.Exceptions;
using TellDontAskKata.Main.UseCase;

namespace TellDontAskKata.Main.Domain
{
    public class Created : OrderStatus
    {
        public override OrderStatus Approve()
        {
            return new Approved();
        }

        public override OrderStatus Reject()
        {
            return new Rejected();
        }

        public override OrderStatus Ship()
        {
            throw new OrderCannotBeShippedException();
        }
    }
}