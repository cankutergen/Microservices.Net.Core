using MediatR;
using Microservice.Product.CQRS.Commands;
using Microservice.Product.CQRS.Queries;
using Microservice.Product.DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microservice.Product.CQRS.Handlers
{
    public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, bool>
    {
        private readonly IProductDal _productDal;
        private readonly IMediator _mediator;

        public DeleteProductHandler(IProductDal productDal, IMediator mediator)
        {
            _productDal = productDal;
            _mediator = mediator;
        }

        public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            // Entity must be exist
            var model = await _mediator.Send(new GetProductByIdQuery(request.Id));
            if (model == null)
            {
                throw new Exception($"Product with id:{request.Id} does not found");
            }

            _productDal.Delete(model);
            return true;
        }
    }
}
