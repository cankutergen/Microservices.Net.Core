using MediatR;
using Microservice.Order.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Order.CQRS.Queries
{
    public class GetOrdersByCustomerIdQuery : IRequest<List<OrderModel>>
    {
        public int customerId { get; set; }

        public GetOrdersByCustomerIdQuery(int customerId)
        {
            this.customerId = customerId;
        }
    }
}
