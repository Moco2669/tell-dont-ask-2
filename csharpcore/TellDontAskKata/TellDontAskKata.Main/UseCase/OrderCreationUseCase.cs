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
            var orderItems = Process(request);
            var order = new Order();
            order.AddItems(orderItems);

            _orderRepository.Save(order);
        }

        private List<OrderItem> Process(SellItemsRequest request)
        {
            List<OrderItem> orderItems = new List<OrderItem>();
            foreach(var itemRequest in request.Requests){
                var product = _productCatalog.GetByName(itemRequest.ProductName);
                orderItems.Add(new OrderItem(product, itemRequest.Quantity));
            }

            return orderItems;
        }
    }
}
