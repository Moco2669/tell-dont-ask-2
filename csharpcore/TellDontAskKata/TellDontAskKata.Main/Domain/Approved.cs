using System;
using TellDontAskKata.Main.Exceptions;

namespace TellDontAskKata.Main.Domain
{
    public class Approved : OrderStatus
    {
        public override OrderStatus Approve()
        {
            return this;
        }

        public override OrderStatus Reject()
        {
            throw new ApprovedOrderCannotBeRejectedException();
        }

        public override OrderStatus Ship()
        {
            return new Shipped();
        }
    }
}