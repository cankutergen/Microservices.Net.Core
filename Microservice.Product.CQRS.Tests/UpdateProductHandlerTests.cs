using FluentValidation;
using MediatR;
using Microservice.Product.Business.ValidationRules.FluentValidation;
using Microservice.Product.CQRS.Commands;
using Microservice.Product.CQRS.Handlers;
using Microservice.Product.CQRS.Queries;
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
    public class UpdateProductHandlerTests
    {
        private readonly Mock<IProductDal> _productDal;
        private readonly IValidator<ProductModel> _validator;
        private readonly Mock<IMediator> _mediator;

        private readonly UpdateProductHandler _updateProductHandler;

        public UpdateProductHandlerTests()
        {
            _productDal = new Mock<IProductDal>();
            _validator = new ProductValidator();
            _mediator = new Mock<IMediator>();

            _updateProductHandler = new UpdateProductHandler(_productDal.Object, _validator, _mediator.Object);
        }

        [Fact]
        public async Task Product_does_not_exist_throws_exception()
        {
            var query = Mock.Of<GetProductByIdQuery>(x => x.Id == 1);

            _mediator.Setup(x => x.Send(query, It.IsAny<CancellationToken>()))
                .Returns((Task<ProductModel>)null);

            var command = Mock.Of<UpdateProductCommand>(x => x.Id == 1);

            Task result() => _updateProductHandler.Handle(command, new CancellationToken());
            await Assert.ThrowsAsync<Exception>(result);
        }

        [Fact]
        public async Task Invalid_model_throws_validation_exception()
        {
            var requestModel = Mock.Of<UpdateProductCommand>(x => x.Id == 1 && x.ImageUrl == "url");
            var query = Mock.Of<GetProductByIdQuery>(x => x.Id == requestModel.Id);

            var entity = Mock.Of<ProductModel>(x =>
                x.Id == 1 &&
                x.Name == "pc" &&
                x.ImageUrl == "url"
            );

            _mediator.Setup(x => x.Send(It.IsAny<GetProductByIdQuery>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(entity));

            Task result() => _updateProductHandler.Handle(requestModel, new CancellationToken());

            await Assert.ThrowsAsync<ValidationException>(result);
        }
    }
}
