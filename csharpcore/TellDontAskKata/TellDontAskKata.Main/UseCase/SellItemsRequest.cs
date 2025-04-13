using System.Collections.Generic;

namespace TellDontAskKata.Main.UseCase
{
    public class SellItemsRequest
    {
        public SellItemsRequest()
        {
        }

        public SellItemsRequest(IList<SellItemRequest> requests)
        {
            Requests = requests;
        }

        public IList<SellItemRequest> Requests { get; set; }
    }
}
