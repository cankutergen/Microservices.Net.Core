using FluentValidation;
using Microservice.Address.Business.ValidationRules.FluentValidation;
using Microservice.Address.CQRS.Commands;
using Microservice.Address.CQRS.Handlers;
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
    public class CreateAddressHandlerTests
    {
        private readonly Mock<IAddressDal> _addressDal;
        private readonly IValidator<AddressModel> _validator;
        private readonly CreateAddressHandler _createAddressHandler;

        public CreateAddressHandlerTests()
        {
            _addressDal = new Mock<IAddressDal>();
            _validator = new AddressValidator();

            _createAddressHandler = new CreateAddressHandler(_addressDal.Object, _validator);
        }

        [Fact]
        public async Task Invalid_model_throws_validation_exception()
        {
            var requestModel = Mock.Of<CreateAddressCommand>(x => x.City == "Bolu");
            var entity = Mock.Of<AddressModel>(x =>
                x.Id == 1 &&
                x.City == "Bolu" &&
                x.CityCode == 1 &&
                x.Country == "Turkey" &&
                x.AddressLine == "Bolu"
            );

            _addressDal.Setup(x => x.Add(entity)).Returns(entity);

            Task result() => _createAddressHandler.Handle(requestModel, new CancellationToken());

            await Assert.ThrowsAsync<ValidationException>(result);
        }
    }
}
