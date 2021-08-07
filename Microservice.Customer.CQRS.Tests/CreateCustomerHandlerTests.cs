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

namespace Microservice.Customer.CQRS.Tests
{
    public class CreateCustomerHandlerTests
    {
        private readonly Mock<ICustomerDal> _customerDal;
        private readonly IValidator<CustomerModel> _validator;
        private readonly CreateCustomerHandler _createCustomerHandler;

        public CreateCustomerHandlerTests()
        {
            _customerDal = new Mock<ICustomerDal>();
            _validator = new CustomerValidator();
            _createCustomerHandler = new CreateCustomerHandler(_customerDal.Object, _validator);
        }

        [Fact]
        public async Task Invalid_model_throws_validation_exception()
        {
            var requestModel = Mock.Of<CreateCustomerCommand>(x => x.Name == "Cankut");
            var entity = Mock.Of<CustomerModel>(x => 
                x.Id == 1 && 
                x.AddressId == 1 && 
                x.CreatedAt == DateTime.Now && 
                x.Email == "test@abc.com" && 
                x.Name == "Cankut" && 
                x.UpdatedAt == DateTime.Now
            );

            _customerDal.Setup(x => x.Add(entity)).Returns(entity);

            Task result() => _createCustomerHandler.Handle(requestModel, new CancellationToken());

            await Assert.ThrowsAsync<ValidationException>(result);
        }

        [Fact]
        public async Task Valid_model()
        {
            var requestModel = Mock.Of<CreateCustomerCommand>(x => 
                x.Name == "Cankut" && 
                x.AddressId == 1 &&
                x.CreatedAt == DateTime.Now &&
                x.Email == "test@abc.com" &&
                x.UpdatedAt == DateTime.Now
            );

            var entity = Mock.Of<CustomerModel>(x =>
                x.AddressId == 1 &&
                x.CreatedAt == DateTime.Now &&
                x.Email == "test@abc.com" &&
                x.Name == "Cankut" &&
                x.UpdatedAt == DateTime.Now
            );

            _customerDal.Setup(x => x.Add(It.IsAny<CustomerModel>()))
                .Returns(entity);

            var expected = await _createCustomerHandler.Handle(requestModel, new CancellationToken());

            Assert.NotNull(expected);
            Assert.Equal(expected, entity);
        }
    }
}
