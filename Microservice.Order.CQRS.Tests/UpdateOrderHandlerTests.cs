using FluentValidation;
using MediatR;
using Microservice.Order.Business.ValidationRules.FluentValidation;
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
    public class UpdateOrderHandlerTests
    {
        private readonly Mock<IOrderDal> _orderDal;
        private readonly Mock<IMediator> _mediator;
        private readonly IValidator<OrderModel> _validator;
        private readonly UpdateOrderHandler _updateOrderHandler;

        public UpdateOrderHandlerTests()
        {
            _orderDal = new Mock<IOrderDal>();
            _mediator = new Mock<IMediator>();
            _validator = new OrderValidator();

            _updateOrderHandler = new UpdateOrderHandler(_orderDal.Object, _mediator.Object, _validator);
        }

        [Fact]
        public async Task Order_does_not_exist_throws_exception()
        {
            var query = Mock.Of<GetOrderByIdQuery>(x => x.Id == 1);

            _mediator.Setup(x => x.Send(query, It.IsAny<CancellationToken>()))
                .Returns((Task<OrderModel>)null);

            var command = Mock.Of<UpdateOrderCommand>(x => x.Id == 1);

            Task result() => _updateOrderHandler.Handle(command, new CancellationToken());
            await Assert.ThrowsAsync<Exception>(result);
        }

        [Fact]
        public async Task Invalid_model_throws_validation_exception()
        {
            var requestModel = Mock.Of<UpdateOrderCommand>(x => x.Id == 1 && x.Quantity == 123);
            var query = Mock.Of<GetOrderByIdQuery>(x => x.Id == requestModel.Id);

            var entity = Mock.Of<OrderModel>(x =>
                x.AddressId == 1 &&
                x.CreatedAt == DateTime.Now &&
                x.CustomerId == 1 &&
                x.Price == 123 &&
                x.ProductId == 1 &&
                x.Status == "OK" &&
                x.Id == 1 &&
                x.UpdatedAt == DateTime.Now &&
                x.Quantity == 100
            );

            _mediator.Setup(x => x.Send(It.IsAny<GetOrderByIdQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(entity));


            Task result() => _updateOrderHandler.Handle(requestModel, new CancellationToken());

            await Assert.ThrowsAsync<ValidationException>(result);
        }
    }
}
