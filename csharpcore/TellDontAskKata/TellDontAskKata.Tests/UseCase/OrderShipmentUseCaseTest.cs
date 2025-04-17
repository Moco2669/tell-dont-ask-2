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
        private readonly Order _anOrder;

        private const int AnOrderId = 1;

        public OrderShipmentUseCaseTest()
        {
            _orderRepository = new TestOrderRepository();
            _shipmentService = new TestShipmentService();
            _useCase = new OrderShipmentUseCase(_orderRepository, _shipmentService);
            _anOrder = new Order(AnOrderId);
            _orderRepository.AddOrder(_anOrder);
        }
        
        [Fact]
        public void ShipApprovedOrder()
        {
            var approvalRequest = new ApproveRequest(AnOrderId);
            approvalRequest.ExecuteOn(_anOrder);

            var request = new ShipRequest(AnOrderId);

            _useCase.Run(request);

            Assert.True(_orderRepository.GetSavedOrder().StatusIs(new Shipped()));
            Assert.Same(_anOrder, _shipmentService.GetShippedOrder());
        }

        [Fact]
        public void CreatedOrdersCannotBeShipped()
        {
            var request = new ShipRequest(AnOrderId);

            Action actionToTest = () => _useCase.Run(request);

            Assert.Throws<OrderCannotBeShippedException>(actionToTest);
            Assert.Null(_orderRepository.GetSavedOrder());
            Assert.Null(_shipmentService.GetShippedOrder());
        }

        [Fact]
        public void RejectedOrdersCannotBeShipped()
        {
            var approvalRequest = new RejectRequest(AnOrderId);
            approvalRequest.ExecuteOn(_anOrder);

            var request = new ShipRequest(AnOrderId);

            Action actionToTest = () => _useCase.Run(request);

            Assert.Throws<OrderCannotBeShippedException>(actionToTest);
            Assert.Null(_orderRepository.GetSavedOrder());
            Assert.Null(_shipmentService.GetShippedOrder());
        }

        [Fact]
        public void ShippedOrdersCannotBeShippedAgain()
        {
            var approvalRequest = new ApproveRequest(AnOrderId);
            approvalRequest.ExecuteOn(_anOrder);
            _anOrder.Ship();

            var request = new ShipRequest(AnOrderId);

            Action actionToTest = () => _useCase.Run(request);

            Assert.Throws<OrderCannotBeShippedTwiceException>(actionToTest);
            Assert.Null(_orderRepository.GetSavedOrder());
            Assert.Null(_shipmentService.GetShippedOrder());
        }
    }
}
