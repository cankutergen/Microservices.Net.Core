using MediatR;
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
    public class DeleteProductHandlerTests
    {
        private readonly Mock<IProductDal> _productDal;
        private readonly Mock<IMediator> _mediator;
        private readonly DeleteProductHandler _deleteProductHandler;

        public DeleteProductHandlerTests()
        {
            _productDal = new Mock<IProductDal>();
            _mediator = new Mock<IMediator>();
            _deleteProductHandler = new DeleteProductHandler(_productDal.Object, _mediator.Object);
        }

        [Fact]
        public async Task Product_does_not_exist_throws_exception()
        {
            var query = Mock.Of<GetProductByIdQuery>(x => x.Id == 1);

            _mediator.Setup(x => x.Send(query, It.IsAny<CancellationToken>()))
                .Returns((Task<ProductModel>)null);

            var command = Mock.Of<DeleteProductCommand>(x => x.Id == 1);

            Task result() => _deleteProductHandler.Handle(command, new CancellationToken());
            await Assert.ThrowsAsync<Exception>(result);
        }
    }
}
