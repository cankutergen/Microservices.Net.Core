using FluentValidation;
using Microservice.Product.Business.ValidationRules.FluentValidation;
using Microservice.Product.CQRS.Commands;
using Microservice.Product.CQRS.Handlers;
using Microservice.Product.DataAccess.Abstract;
using Microservice.Product.Entities.Concrete;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Microservice.Product.CQRS.Tests
{
    public class CreateProductHandlerTests
    {
        private readonly Mock<IProductDal> _productDal;
        private readonly IValidator<ProductModel> _validator;
        private readonly CreateProductHandler _createAddressHandler;

        public CreateProductHandlerTests()
        {
            _productDal = new Mock<IProductDal>();
            _validator = new ProductValidator();

            _createAddressHandler = new CreateProductHandler(_productDal.Object, _validator);
        }

        [Fact]
        public async Task Invalid_model_throws_validation_exception()
        {
            var requestModel = Mock.Of<CreateProductCommand>(x => x.ImageUrl == "url");
            var entity = Mock.Of<ProductModel>(x =>
                x.Id == 1 &&
                x.Name == "PC" &&
                x.ImageUrl == "url"
            );

            _productDal.Setup(x => x.Add(entity)).Returns(entity);

            Task result() => _createAddressHandler.Handle(requestModel, new CancellationToken());

            await Assert.ThrowsAsync<ValidationException>(result);
        }
    }
}
