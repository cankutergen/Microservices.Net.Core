using MediatR;
using Microservice.Product.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Product.CQRS.Queries
{
    public class GetProductByIdQuery : IRequest<ProductModel>
    {
        public int Id { get; set; }

        public GetProductByIdQuery()
        {

        }
        public GetProductByIdQuery(int ıd)
        {
            Id = ıd;
        }
    }
}
