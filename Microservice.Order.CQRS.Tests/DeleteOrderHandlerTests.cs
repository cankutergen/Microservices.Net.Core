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
    public class DeleteOrderHandlerTests
    {
        private readonly Mock<IOrderDal> _orderDal;
        private readonly Mock<IMediator> _mediator;
        private readonly DeleteOrderHandler _deleteOrderHandler;

        public DeleteOrderHandlerTests()
        {
            _orderDal = new Mock<IOrderDal>();
            _mediator = new Mock<IMediator>();
            _deleteOrderHandler = new DeleteOrderHandler(_orderDal.Object, _mediator.Object);
        }

        [Fact]
        public async Task Order_does_not_exist_throws_exception()
        {
            var query = Mock.Of<GetOrderByIdQuery>(x => x.Id == 1);

            _mediator.Setup(x => x.Send(query, It.IsAny<CancellationToken>()))
                .Returns((Task<OrderModel>)null);

            var command = Mock.Of<DeleteOrderCommand>(x => x.Id == 1);

            Task result() => _deleteOrderHandler.Handle(command, new CancellationToken());
            await Assert.ThrowsAsync<Exception>(result);
        }
    }
}
