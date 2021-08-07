using MediatR;
using Microservice.Address.CQRS.Commands;
using Microservice.Address.CQRS.Handlers;
using Microservice.Address.CQRS.Queries;
using Microservice.Address.DataAccess.Abstract;
using Microservice.Address.Entities.Concrete;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Microservice.Address.CQRS.Tests
{
    public class DeleteAddressHandlerTests
    {
        private readonly Mock<IAddressDal> _addressDal;
        private readonly Mock<IMediator> _mediator;
        private readonly DeleteAddressHandler _deleteAddressHandler;

        public DeleteAddressHandlerTests()
        {
            _addressDal = new Mock<IAddressDal>();
            _mediator = new Mock<IMediator>();

            _deleteAddressHandler = new DeleteAddressHandler(_addressDal.Object, _mediator.Object);
        }

        [Fact]
        public async Task Address_does_not_exist_throws_exception()
        {
            var query = Mock.Of<GetAddresByIdQuery>(x => x.Id == 1);

            _mediator.Setup(x => x.Send(query, It.IsAny<CancellationToken>()))
                .Returns((Task<AddressModel>)null);

            var command = Mock.Of<DeleteAddressCommand>(x => x.Id == 1);

            Task result() => _deleteAddressHandler.Handle(command, new CancellationToken());
            await Assert.ThrowsAsync<Exception>(result);
        }
    }
}
