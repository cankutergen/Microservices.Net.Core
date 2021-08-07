using FluentValidation;
using MediatR;
using Microservice.Order.CQRS.Commands;
using Microservice.Order.CQRS.Queries;
using Microservice.Order.DataAccess.Abstract;
using Microservice.Order.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tesodev.Core.CrossCuttingConcerns.FluentValidation;

namespace Microservice.Order.CQRS.Handlers
{
    public class UpdateOrderHandler : IRequestHandler<UpdateOrderCommand, bool>
    {
        private readonly IOrderDal _orderDal;
        private readonly IMediator _mediator;
        private readonly IValidator<OrderModel> _validator;

        public UpdateOrderHandler(IOrderDal orderDal, IMediator mediator, IValidator<OrderModel> validator)
        {
            _orderDal = orderDal;
            _mediator = mediator;
            _validator = validator;
        }

        public async Task<bool> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var model = new OrderModel
            {
                AddressId = request.AddressId,
                CreatedAt = request.CreatedAt,
                CustomerId = request.CustomerId,
                Id = request.Id,
                Price = request.Price,
                UpdatedAt = request.UpdatedAt,
                Quantity = request.Quantity,
                ProductId = request.ProductId,
                Status = request.Status
            };

            // Entity must be exist
            var entity = await _mediator.Send(new GetOrderByIdQuery(request.Id));
            if (entity == null)
            {
                throw new Exception($"Order with id:{request.Id} does not found");
            }

            // If model is invalid, it will thow exception
            ValidatorTool.FluentValidate(_validator, model);
            await Task.FromResult(_orderDal.Update(model));
            return true;
        }
    }
}
