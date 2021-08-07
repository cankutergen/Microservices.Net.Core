using MediatR;
using Microservice.Order.CQRS.Queries;
using Microservice.Order.DataAccess.Abstract;
using Microservice.Order.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microservice.Order.CQRS.Handlers
{
    public class GetOrdersByCustomerIdHandler : IRequestHandler<GetOrdersByCustomerIdQuery, List<OrderModel>>
    {
        private readonly IOrderDal _orderDal;

        public GetOrdersByCustomerIdHandler(IOrderDal orderDal)
        {
            _orderDal = orderDal;
        }

        public Task<List<OrderModel>> Handle(GetOrdersByCustomerIdQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_orderDal.GetList(x => x.CustomerId == request.customerId));
        }
    }
}
