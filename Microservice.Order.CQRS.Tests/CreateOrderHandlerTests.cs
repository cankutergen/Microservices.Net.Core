using FluentValidation;
using Microservice.Order.Business.ValidationRules.FluentValidation;
using Microservice.Order.CQRS.Commands;
using Microservice.Order.CQRS.Handlers;
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
    public class CreateOrderHandlerTests
    {
        private readonly Mock<IOrderDal> _orderDal;
        private readonly IValidator<OrderModel> _validator;
        private readonly CreateOrderHandler _createOrderHandler;

        public CreateOrderHandlerTests()
        {
            _orderDal = new Mock<IOrderDal>();
            _validator = new OrderValidator();
            _createOrderHandler = new CreateOrderHandler(_orderDal.Object, _validator);
        }

        [Fact]
        public async Task Invalid_model_throws_validation_exception()
        {
            var requestModel = Mock.Of<CreateOrderCommand>(x => x.Price == 123);

            _orderDal.Setup(x => x.Add(It.IsAny<OrderModel>())).Returns(It.IsAny<OrderModel>());

            Task result() => _createOrderHandler.Handle(requestModel, new CancellationToken());

            await Assert.ThrowsAsync<ValidationException>(result);
        }
    }
}
