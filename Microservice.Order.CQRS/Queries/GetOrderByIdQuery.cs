using MediatR;
using Microservice.Order.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Order.CQRS.Queries
{
    public class GetOrderByIdQuery : IRequest<OrderModel>
    {
        public int Id { get; set; }

        public GetOrderByIdQuery()
        {

        }

        public GetOrderByIdQuery(int id)
        {
            Id = id;
        }
    }
}
