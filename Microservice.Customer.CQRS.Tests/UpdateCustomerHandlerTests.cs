using FluentValidation;
using MediatR;
using Microservice.Customer.Business.ValidationRules.FluentValidation;
using Microservice.Customer.CQRS.Commands;
using Microservice.Customer.CQRS.Handlers;
using Microservice.Customer.DataAccess.Abstract;
using Microservice.Customer.Entities.Concrete;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Microservice.Customer.CQRS.Queries
{
    public class UpdateCustomerHandlerTests
    {
        private readonly Mock<ICustomerDal> _customerDal;
        private readonly Mock<IMediator> _mediator;
        private readonly IValidator<CustomerModel> _validator;

        private readonly UpdateCustomerHandler _updateCustomerHandler;

        public UpdateCustomerHandlerTests()
        {
            _customerDal = new Mock<ICustomerDal>();
            _mediator = new Mock<IMediator>();
            _validator = new CustomerValidator();

            _updateCustomerHandler = new UpdateCustomerHandler(_customerDal.Object, _mediator.Object, _validator);
        }

        [Fact]
        public async Task Customer_does_not_exist_throws_exception()
        {
            var query = Mock.Of<GetCustomerByIdQuery>(x => x.Id == 1);

            _mediator.Setup(x => x.Send(query, It.IsAny<CancellationToken>()))
                .Returns((Task<CustomerModel>)null);

            var command = Mock.Of<UpdateCustomerCommand>(x => x.Id == 1);

            Task result() => _updateCustomerHandler.Handle(command, new CancellationToken());
            await Assert.ThrowsAsync<Exception>(result);
        }

        [Fact]
        public async Task Invalid_model_throws_validation_exception()
        {
            var requestModel = Mock.Of<UpdateCustomerCommand>(x => x.Id == 1 && x.Name == "Cankut");
            var query = Mock.Of<GetCustomerByIdQuery>(x => x.Id == requestModel.Id);

            var entity = Mock.Of<CustomerModel>(x =>
                x.AddressId == 1 &&
                x.CreatedAt == DateTime.Now &&
                x.Email == "test@abc.com" &&
                x.Name == "Cankut" &&
                x.UpdatedAt == DateTime.Now
            );

            _mediator.Setup(x =>  x.Send(It.IsAny<GetCustomerByIdQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(entity));


            Task result() => _updateCustomerHandler.Handle(requestModel, new CancellationToken());

            await Assert.ThrowsAsync<ValidationException>(result);
        }
    }
}
