using MediatR;
using Microservice.Order.CQRS.Commands;
using Microservice.Order.CQRS.Handlers;
using Microservice.Order.CQRS.Queries;
using Microservice.Order.DataAccess.Abstract;
using Microservice.Order.Entities.Concrete;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Microservice.Order.CQRS.Tests
{
    public class UpdateOrderStatusHandlerTests
    {
        private readonly Mock<IOrderDal> _orderDal;
        private readonly Mock<IMediator> _mediator;
        private readonly UpdateOrderStatusHandler _updateOrderStatusHandler;

        public UpdateOrderStatusHandlerTests()
        {
            _orderDal = new Mock<IOrderDal>();
            _mediator = new Mock<IMediator>();

            _updateOrderStatusHandler = new UpdateOrderStatusHandler(_orderDal.Object, _mediator.Object);
        }

        [Fact]
        public async Task Order_does_not_exist_throws_exception()
        {
            var query = Mock.Of<GetOrderByIdQuery>(x => x.Id == 1);

            _mediator.Setup(x => x.Send(query, It.IsAny<CancellationToken>()))
                .Returns((Task<OrderModel>)null);

            var command = Mock.Of<UpdateOrderStatusCommand>(x => x.Id == 1 && x.Status == "OK");

            Task result() => _updateOrderStatusHandler.Handle(command, new CancellationToken());
            await Assert.ThrowsAsync<Exception>(result);
        }
    }
}
