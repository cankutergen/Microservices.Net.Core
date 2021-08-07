using MediatR;
using Microservice.Order.CQRS.Commands;
using Microservice.Order.CQRS.Queries;
using Microservice.Order.DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microservice.Order.CQRS.Handlers
{
    public class DeleteOrderHandler : IRequestHandler<DeleteOrderCommand, bool>
    {
        private readonly IOrderDal _orderDal;
        private readonly IMediator _mediator;

        public DeleteOrderHandler(IOrderDal orderDal, IMediator mediator)
        {
            _orderDal = orderDal;
            _mediator = mediator;
        }

        public async Task<bool> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            // Entity must be exist
            var model = await _mediator.Send(new GetOrderByIdQuery(request.Id));
            if (model == null)
            {
                throw new Exception($"Order with id:{request.Id} does not found");
            }

            _orderDal.Delete(model);
            return true;
        }
    }
}
