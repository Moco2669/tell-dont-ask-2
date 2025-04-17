using TellDontAskKata.Main.Exceptions;

namespace TellDontAskKata.Main.Domain
{
    public class Rejected : OrderStatus
    {
        public override OrderStatus Approve()
        {
            throw new RejectedOrderCannotBeApprovedException();
        }

        public override OrderStatus Reject()
        {
            return this;
        }

        public override OrderStatus Ship()
        {
            throw new OrderCannotBeShippedException();
        }
    }
}