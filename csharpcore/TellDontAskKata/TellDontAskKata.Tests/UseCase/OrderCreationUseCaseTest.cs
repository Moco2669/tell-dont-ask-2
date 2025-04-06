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

        public OrderCreationUseCaseTest()
        {
            var food = new Category { 
                Name = CategoryName,
                TaxPercentage = CategoryTaxPercentage
            };

            _productCatalog = new InMemoryProductCatalog(new List<Product>
            {
                new Product
                {
                    Name = Product1Name,
                    Price = Product1Price,
                    Category = food
                },
                new Product
                {
                    Name = Product2Name,
                    Price = Product2Price,
                    Category = food
                }
            });

            _orderRepository = new TestOrderRepository();

            _useCase = new OrderCreationUseCase(_orderRepository, _productCatalog);
        }


        [Fact]
        public void SellMultipleItems()
        {
            const int saladQuantity = 2;
            var saladRequest = new SellItemRequest
            {
                ProductName = Product1Name,
                Quantity = saladQuantity
            };

            const int tomatoQuantity = 3;
            var tomatoRequest = new SellItemRequest
            {
                ProductName = Product2Name,
                Quantity = tomatoQuantity
            };

            var request = new SellItemsRequest
            {
                Requests = new List<SellItemRequest> { saladRequest, tomatoRequest }
            };

            _useCase.Run(request);

            Order insertedOrder = _orderRepository.GetSavedOrder();
            Assert.Equal(OrderStatus.Created, insertedOrder.Status);
            Assert.Equal(23.20m, insertedOrder.Total);
            Assert.Equal(2.13m, insertedOrder.Tax);
            Assert.Equal("EUR", insertedOrder.Currency);
            Assert.Equal(2, insertedOrder.Items.Count);
            Assert.Equal(Product1Name, insertedOrder.Items[0].Product.Name);
            Assert.Equal(Product1Price, insertedOrder.Items[0].Product.Price);
            Assert.Equal(saladQuantity, insertedOrder.Items[0].Quantity);
            Assert.Equal(7.84m, insertedOrder.Items[0].TaxedAmount);
            Assert.Equal(0.72m, insertedOrder.Items[0].Tax);
            Assert.Equal(Product2Name, insertedOrder.Items[1].Product.Name);
            Assert.Equal(Product2Price, insertedOrder.Items[1].Product.Price);
            Assert.Equal(tomatoQuantity, insertedOrder.Items[1].Quantity);
            Assert.Equal(15.36m, insertedOrder.Items[1].TaxedAmount);
            Assert.Equal(1.41m, insertedOrder.Items[1].Tax);
        }

        [Fact]
        public void UnknownProduct()
        {
            var request = new SellItemsRequest
            {
                Requests = new List<SellItemRequest> { 
                    new SellItemRequest { ProductName = "unknown product"}
                }
            };

            Action actionToTest = () => _useCase.Run(request);

            Assert.Throws<UnknownProductException>(actionToTest);
        }



    }
}
