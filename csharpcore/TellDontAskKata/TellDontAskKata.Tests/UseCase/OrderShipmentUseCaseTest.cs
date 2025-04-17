using System;
using System.ComponentModel;
using TellDontAskKata.Main.Domain;
using TellDontAskKata.Main.Exceptions;
using TellDontAskKata.Main.Requests;
using TellDontAskKata.Main.UseCase;
using TellDontAskKata.Tests.Doubles;
using Xunit;

namespace TellDontAskKata.Tests.UseCase
{
    public class OrderShipmentUseCaseTest
    {
        private readonly TestOrderRepository _orderRepository;
        private readonly TestShipmentService _shipmentService;
        private readonly OrderShipmentUseCase _useCase;

        private const int AnOrderId = 1;

        public OrderShipmentUseCaseTest()
        {
            _orderRepository = new TestOrderRepository();
            _shipmentService = new TestShipmentService();
            _useCase = new OrderShipmentUseCase(_orderRepository, _shipmentService);
        }
        
        [Fact]
        public void ShipApprovedOrder()
        {
            var initialOrder = new Order(AnOrderId);
            var approvalRequest = new ApproveRequest(AnOrderId);
            approvalRequest.ExecuteRequest(initialOrder);
            _orderRepository.AddOrder(initialOrder);

            var request = new ShipRequest(AnOrderId);

            _useCase.Run(request);

            Assert.True(_orderRepository.GetSavedOrder().StatusIs(new Shipped()));
            Assert.Same(initialOrder, _shipmentService.GetShippedOrder());
        }

        [Fact]
        public void CreatedOrdersCannotBeShipped()
        {
            var initialOrder = new Order(AnOrderId);
            _orderRepository.AddOrder(initialOrder);

            var request = new ShipRequest(AnOrderId);

            Action actionToTest = () => _useCase.Run(request);

            Assert.Throws<OrderCannotBeShippedException>(actionToTest);
            Assert.Null(_orderRepository.GetSavedOrder());
            Assert.Null(_shipmentService.GetShippedOrder());
        }

        [Fact]
        public void RejectedOrdersCannotBeShipped()
        {
            var initialOrder = new Order(AnOrderId);
            var approvalRequest = new RejectRequest(AnOrderId);
            approvalRequest.ExecuteRequest(initialOrder);
            _orderRepository.AddOrder(initialOrder);

            var request = new ShipRequest(AnOrderId);

            Action actionToTest = () => _useCase.Run(request);

            Assert.Throws<OrderCannotBeShippedException>(actionToTest);
            Assert.Null(_orderRepository.GetSavedOrder());
            Assert.Null(_shipmentService.GetShippedOrder());
        }

        [Fact]
        public void ShippedOrdersCannotBeShippedAgain()
        {
            var initialOrder = new Order(AnOrderId);
            var approvalRequest = new ApproveRequest(AnOrderId);
            approvalRequest.ExecuteRequest(initialOrder);
            initialOrder.Ship();
            _orderRepository.AddOrder(initialOrder);

            var request = new ShipRequest(AnOrderId);

            Action actionToTest = () => _useCase.Run(request);

            Assert.Throws<OrderCannotBeShippedTwiceException>(actionToTest);
            Assert.Null(_orderRepository.GetSavedOrder());
            Assert.Null(_shipmentService.GetShippedOrder());
        }
    }
}
