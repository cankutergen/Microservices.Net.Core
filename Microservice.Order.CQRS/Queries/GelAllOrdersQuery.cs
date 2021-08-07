using MediatR;
using Microservice.Order.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Order.CQRS.Queries
{
    public class GelAllOrdersQuery : IRequest<List<OrderModel>>
    {
    }
}
