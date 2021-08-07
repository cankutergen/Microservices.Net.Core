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
    public class UpdateOrderStatusHandler : IRequestHandler<UpdateOrderStatusCommand, bool>
    {

        private readonly IOrderDal _orderDal;
        private readonly IMediator _mediator;

        public UpdateOrderStatusHandler(IOrderDal orderDal, IMediator mediator)
        {
            _orderDal = orderDal;
            _mediator = mediator;
        }

        public async Task<bool> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
        {
            // Entity must be exist
            var entity = await _mediator.Send(new GetOrderByIdQuery(request.Id));
            if (entity == null)
            {
                throw new Exception($"Order with id:{request.Id} does not found");
            }

            entity.Status = request.Status;
            await Task.FromResult(_orderDal.Update(entity));
            return true;
        }
    }
}
