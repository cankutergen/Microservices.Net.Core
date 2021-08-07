using FluentValidation;
using MediatR;
using Microservice.Address.Business.ValidationRules.FluentValidation;
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
    public class UpdateAddressHandlerTests
    {
        private readonly Mock<IAddressDal> _addressDal;
        private readonly IValidator<AddressModel> _validator;
        private readonly Mock<IMediator> _mediator;

        private readonly UpdateAddressHandler _updateAddressHandler;

        public UpdateAddressHandlerTests()
        {
            _addressDal = new Mock<IAddressDal>();
            _validator = new AddressValidator();
            _mediator = new Mock<IMediator>();

            _updateAddressHandler = new UpdateAddressHandler(_addressDal.Object, _validator, _mediator.Object);
        }

        [Fact]
        public async Task Address_does_not_exist_throws_exception()
        {
            var query = Mock.Of<GetAddresByIdQuery>(x => x.Id == 1);

            _mediator.Setup(x => x.Send(query, It.IsAny<CancellationToken>()))
                .Returns((Task<AddressModel>)null);

            var command = Mock.Of<UpdateAddressCommand>(x => x.Id == 1);

            Task result() => _updateAddressHandler.Handle(command, new CancellationToken());
            await Assert.ThrowsAsync<Exception>(result);
        }

        [Fact]
        public async Task Invalid_model_throws_validation_exception()
        {
            var requestModel = Mock.Of<UpdateAddressCommand>(x => x.Id == 1 && x.City == "Bolu");
            var query = Mock.Of<GetAddresByIdQuery>(x => x.Id == requestModel.Id);

            var entity = Mock.Of<AddressModel>(x =>
                x.Id == 1 &&
                x.City == "Bolu" &&
                x.CityCode == 1 &&
                x.Country == "Turkey" &&
                x.AddressLine == "Bolu"
            );

            _mediator.Setup(x => x.Send(It.IsAny<GetAddresByIdQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(entity));

            Task result() => _updateAddressHandler.Handle(requestModel, new CancellationToken());

            await Assert.ThrowsAsync<ValidationException>(result);
        }
    }
}
