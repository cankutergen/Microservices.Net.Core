using MediatR;
using Microservice.Customer.CQRS.Commands;
using Microservice.Customer.CQRS.Handlers;
using Microservice.Customer.CQRS.Queries;
using Microservice.Customer.DataAccess.Abstract;
using Microservice.Customer.Entities.Concrete;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Microservice.Customer.CQRS.Tests
{
    public class DeleteCustomerHandlerTests
    {
        private readonly Mock<ICustomerDal> _customerDal;
        private readonly Mock<IMediator> _mediator;
        private readonly DeleteCustomerHandler _deleteCustomerHandler;

        public DeleteCustomerHandlerTests()
        {
            _customerDal = new Mock<ICustomerDal>();
            _mediator = new Mock<IMediator>();

            _deleteCustomerHandler = new DeleteCustomerHandler(_customerDal.Object, _mediator.Object);
        }

        [Fact]
        public async Task Customer_does_not_exist_throws_exception()
        {
            var query = Mock.Of<GetCustomerByIdQuery>(x => x.Id == 1);

            _mediator.Setup(x => x.Send(query, It.IsAny<CancellationToken>()))
                .Returns((Task<CustomerModel>)null);

            var command = Mock.Of<DeleteCustomerCommand>(x => x.Id == 1);

            Task result() => _deleteCustomerHandler.Handle(command, new CancellationToken());
            await Assert.ThrowsAsync<Exception>(result);
        } 
    }
}
