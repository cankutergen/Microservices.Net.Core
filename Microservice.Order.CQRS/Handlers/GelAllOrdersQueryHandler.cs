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
    public class GelAllOrdersQueryHandler : IRequestHandler<GelAllOrdersQuery, List<OrderModel>>
    {
        private readonly IOrderDal _orderDal;

        public GelAllOrdersQueryHandler(IOrderDal orderDal)
        {
            _orderDal = orderDal;
        }

        public Task<List<OrderModel>> Handle(GelAllOrdersQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_orderDal.GetList());
        }
    }
}
