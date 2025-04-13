using System;
using System.Collections.Generic;
using TellDontAskKata.Main.Domain;
using TellDontAskKata.Main.Repository;
using TellDontAskKata.Main.UseCase;
using TellDontAskKata.Tests.Doubles;
using Xunit;

namespace TellDontAskKata.Tests.UseCase
{
    public class OrderCreationUseCaseTest
    {
        private readonly TestOrderRepository _orderRepository;
        private readonly IProductCatalog _productCatalog;
        private readonly OrderCreationUseCase _useCase;

        private const string CategoryName = "food";
        private const decimal CategoryTaxPercentage = 10m;
        private const string Product1Name = "salad";
        private const decimal Product1Price = 3.56m;
        private const string Product2Name = "tomato";
        private const decimal Product2Price = 4.65m;
        private const int SaladQuantity = 2;
        private const int TomatoQuantity = 3;

        public OrderCreationUseCaseTest()
        {
            var food = new Category(CategoryName, CategoryTaxPercentage);

            var products = new List<Product>()
            {
                new Product(Product1Name, Product1Price, food),
                new Product(Product2Name, Product2Price, food)
            };

            _productCatalog = new InMemoryProductCatalog(products);
            _orderRepository = new TestOrderRepository();
            _useCase = new OrderCreationUseCase(_orderRepository, _productCatalog);
        }


        [Fact]
        public void SellMultipleItems()
        {
            var saladRequest = new SellItemRequest(Product1Name, SaladQuantity);
            var tomatoRequest = new SellItemRequest(Product2Name, TomatoQuantity);

            var requests = new List<SellItemRequest> { saladRequest, tomatoRequest };
            var request = new SellItemsRequest(requests);
            
            _useCase.Run(request);

            Order insertedOrder = _orderRepository.GetSavedOrder();
            Assert.Equal(OrderStatus.Created, insertedOrder.Status);
            Assert.Equal(23.20m, insertedOrder.Total);
            Assert.Equal(2.13m, insertedOrder.Tax);
            Assert.Equal("EUR", insertedOrder.Currency);
            Assert.Equal(requests.Count, insertedOrder.Items.Count);
            Assert.Equal(Product1Name, insertedOrder.Items[0].Product.Name);
            Assert.Equal(Product1Price, insertedOrder.Items[0].Product.Price);
            Assert.Equal(SaladQuantity, insertedOrder.Items[0].Quantity);
            Assert.Equal(7.84m, insertedOrder.Items[0].TaxedAmount);
            Assert.Equal(0.72m, insertedOrder.Items[0].Tax);
            Assert.Equal(Product2Name, insertedOrder.Items[1].Product.Name);
            Assert.Equal(Product2Price, insertedOrder.Items[1].Product.Price);
            Assert.Equal(TomatoQuantity, insertedOrder.Items[1].Quantity);
            Assert.Equal(15.36m, insertedOrder.Items[1].TaxedAmount);
            Assert.Equal(1.41m, insertedOrder.Items[1].Tax);
        }

        [Fact]
        public void UnknownProduct()
        {
            var unknownProductName = "unknown product";
            var requests = new List<SellItemRequest>() { new SellItemRequest(unknownProductName) };
            var request = new SellItemsRequest(requests);

            Action actionToTest = () => _useCase.Run(request);

            Assert.Throws<UnknownProductException>(actionToTest);
        }
    }
}
