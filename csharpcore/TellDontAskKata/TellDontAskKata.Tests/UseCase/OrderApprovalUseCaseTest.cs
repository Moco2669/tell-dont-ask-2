using System;
using TellDontAskKata.Main.Domain;
using TellDontAskKata.Main.UseCase;
using TellDontAskKata.Tests.Doubles;
using Xunit;

namespace TellDontAskKata.Tests.UseCase
{
    public class OrderApprovalUseCaseTest
    {
        private readonly TestOrderRepository _orderRepository;
        private readonly OrderApprovalUseCase _useCase;

        private const int AnOrderId = 1;

        public OrderApprovalUseCaseTest()
        {
            _orderRepository = new TestOrderRepository();
            _useCase = new OrderApprovalUseCase(_orderRepository);
        }


        [Fact]
        public void ApprovedExistingOrder()
        {
            var initialOrder = new Order(AnOrderId);
            _orderRepository.AddOrder(initialOrder);

            var request = new OrderApprovalRequest(AnOrderId, true);

            _useCase.Run(request);

            var savedOrder = _orderRepository.GetSavedOrder();
            Assert.Equal(OrderStatus.Approved, savedOrder.Status);
        }

        [Fact]
        public void RejectedExistingOrder()
        {
            var initialOrder = new Order(AnOrderId);
            _orderRepository.AddOrder(initialOrder);

            var request = new OrderApprovalRequest(AnOrderId, false);

            _useCase.Run(request);

            var savedOrder = _orderRepository.GetSavedOrder();
            Assert.Equal(OrderStatus.Rejected, savedOrder.Status);
        }


        [Fact]
        public void CannotApproveRejectedOrder()
        {
            var initialOrder = new Order
            {
                Status = OrderStatus.Rejected,
                Id = AnOrderId
            };
            _orderRepository.AddOrder(initialOrder);

            var request = new OrderApprovalRequest(AnOrderId, true);
            
            Action actionToTest = () => _useCase.Run(request);
      
            Assert.Throws<RejectedOrderCannotBeApprovedException>(actionToTest);
            Assert.Null(_orderRepository.GetSavedOrder());
        }

        [Fact]
        public void CannotRejectApprovedOrder()
        {
            var initialOrder = new Order
            {
                Status = OrderStatus.Approved,
                Id = AnOrderId
            };
            _orderRepository.AddOrder(initialOrder);

            var request = new OrderApprovalRequest(AnOrderId, false);


            Action actionToTest = () => _useCase.Run(request);
            
            Assert.Throws<ApprovedOrderCannotBeRejectedException>(actionToTest);
            Assert.Null(_orderRepository.GetSavedOrder());
        }

        [Fact]
        public void ShippedOrdersCannotBeRejected()
        {
            var initialOrder = new Order
            {
                Status = OrderStatus.Shipped,
                Id = AnOrderId
            };
            _orderRepository.AddOrder(initialOrder);

            var request = new OrderApprovalRequest(AnOrderId, false);

            Action actionToTest = () => _useCase.Run(request);

            Assert.Throws<ShippedOrdersCannotBeChangedException>(actionToTest);
            Assert.Null(_orderRepository.GetSavedOrder());
        }

    }
}
