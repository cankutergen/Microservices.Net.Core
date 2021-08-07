using MediatR;
using Microservice.Product.CQRS.Queries;
using Microservice.Product.DataAccess.Abstract;
using Microservice.Product.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microservice.Product.CQRS.Handlers
{
    public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, ProductModel>
    {
        private readonly IProductDal _productDal;

        public GetProductByIdHandler(IProductDal productDal)
        {
            _productDal = productDal;
        }

        public Task<ProductModel> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_productDal.Get(x => x.Id == request.Id));
        }
    }
}
