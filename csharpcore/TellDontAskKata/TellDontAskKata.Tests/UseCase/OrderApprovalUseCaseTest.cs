using System;
using TellDontAskKata.Main.Domain;
using TellDontAskKata.Main.Exceptions;
using TellDontAskKata.Main.Requests;
using TellDontAskKata.Main.UseCase;
using TellDontAskKata.Tests.Doubles;
using Xunit;

namespace TellDontAskKata.Tests.UseCase
{
    public class OrderApprovalUseCaseTest
    {
        private readonly TestOrderRepository _orderRepository;
        private readonly OrderApprovalUseCase _useCase;
        private readonly Order _anOrder;

        private const int AnOrderId = 1;

        public OrderApprovalUseCaseTest()
        {
            _orderRepository = new TestOrderRepository();
            _useCase = new OrderApprovalUseCase(_orderRepository);
            _anOrder = new Order(AnOrderId);
        }


        [Fact]
        public void ApprovedExistingOrder()
        {
            _orderRepository.AddOrder(_anOrder);

            var request = new ApproveRequest(AnOrderId);

            _useCase.Run(request);

            var savedOrder = _orderRepository.GetSavedOrder();
            Assert.True(savedOrder.StatusIs(new Approved()));
        }

        [Fact]
        public void RejectedExistingOrder()
        {
            _orderRepository.AddOrder(_anOrder);

            var request = new RejectRequest(AnOrderId);

            _useCase.Run(request);

            var savedOrder = _orderRepository.GetSavedOrder();
            Assert.True(savedOrder.StatusIs(new Rejected()));
        }


        [Fact]
        public void CannotApproveRejectedOrder()
        {
            var approvalRequest = new RejectRequest(AnOrderId);
            approvalRequest.ExecuteOn(_anOrder);
            _orderRepository.AddOrder(_anOrder);

            var request = new ApproveRequest(AnOrderId);
            
            Action actionToTest = () => _useCase.Run(request);
      
            Assert.Throws<RejectedOrderCannotBeApprovedException>(actionToTest);
            Assert.Null(_orderRepository.GetSavedOrder());
        }

        [Fact]
        public void CannotRejectApprovedOrder()
        {
            var approvalRequest = new ApproveRequest(AnOrderId);
            approvalRequest.ExecuteOn(_anOrder);
            _orderRepository.AddOrder(_anOrder);

            var request = new RejectRequest(AnOrderId);

            Action actionToTest = () => _useCase.Run(request);
            
            Assert.Throws<ApprovedOrderCannotBeRejectedException>(actionToTest);
            Assert.Null(_orderRepository.GetSavedOrder());
        }

        [Fact]
        public void ShippedOrdersCannotBeRejected()
        {
            var approvalRequest = new ApproveRequest(AnOrderId);
            approvalRequest.ExecuteOn(_anOrder);
            _anOrder.Ship();
            _orderRepository.AddOrder(_anOrder);

            var request = new RejectRequest(AnOrderId);

            Action actionToTest = () => _useCase.Run(request);

            Assert.Throws<ShippedOrdersCannotBeChangedException>(actionToTest);
            Assert.Null(_orderRepository.GetSavedOrder());
        }

    }
}
