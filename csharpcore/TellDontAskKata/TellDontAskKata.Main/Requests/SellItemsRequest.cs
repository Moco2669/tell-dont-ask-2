using System.Collections.Generic;

namespace TellDontAskKata.Main.Requests
{
    public class SellItemsRequest
    {
        public SellItemsRequest()
        {
        }

        public SellItemsRequest(IList<ItemToOrder> itemsToOrder)
        {
            ItemsToOrder = itemsToOrder;
        }

        public IList<ItemToOrder> ItemsToOrder { get; }
    }
}
