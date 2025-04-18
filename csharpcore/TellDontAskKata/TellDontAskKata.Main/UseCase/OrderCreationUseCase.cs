using System.Collections.Generic;
using TellDontAskKata.Main.Domain;
using TellDontAskKata.Main.Repository;
using TellDontAskKata.Main.Requests;

namespace TellDontAskKata.Main.UseCase
{
    public class OrderCreationUseCase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductCatalog _productCatalog;

        public OrderCreationUseCase(
            IOrderRepository orderRepository,
            IProductCatalog productCatalog)
        {
            _orderRepository = orderRepository;
            _productCatalog = productCatalog;
        }

        public void Run(SellItemsRequest request)
        {
            var order = new Order(ItemsToOrderFrom(request));

            _orderRepository.Save(order);
        }

        private List<OrderItem> ItemsToOrderFrom(SellItemsRequest request)
        {
            List<OrderItem> orderItems = new List<OrderItem>();
            foreach (var itemRequest in request.ItemsToOrder)
            {
                var product = _productCatalog.GetByName(itemRequest.ProductName);
                orderItems.Add(new OrderItem(product, itemRequest.Quantity));
            }

            return orderItems;
        }
    }
}
