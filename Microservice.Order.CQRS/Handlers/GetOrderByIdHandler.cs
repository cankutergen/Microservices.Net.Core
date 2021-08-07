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
    public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdQuery, OrderModel>
    {
        private readonly IOrderDal _orderDal;

        public GetOrderByIdHandler(IOrderDal orderDal)
        {
            _orderDal = orderDal;
        }

        public Task<OrderModel> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_orderDal.Get(x => x.Id == request.Id));
        }
    }
}
