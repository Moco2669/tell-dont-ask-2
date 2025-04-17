using TellDontAskKata.Main.UseCase;

namespace TellDontAskKata.Main.Domain
{
    public abstract class OrderStatus
    {
        public abstract OrderStatus Approve();
        public abstract OrderStatus Reject();
        public abstract OrderStatus Ship();
        public override bool Equals(object obj)
        {
            return obj?.GetType() == GetType();
        }
    }
}
