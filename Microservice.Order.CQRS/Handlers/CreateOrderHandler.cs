using FluentValidation;
using MediatR;
using Microservice.Order.CQRS.Commands;
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
    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, OrderModel>
    {
        private readonly IOrderDal _orderDal;
        private readonly IValidator<OrderModel> _validator;

        public CreateOrderHandler(IOrderDal orderDal, IValidator<OrderModel> validator)
        {
            _orderDal = orderDal;
            _validator = validator;
        }

        public Task<OrderModel> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var entity = new OrderModel
            {
                AddressId = request.AddressId,
                CustomerId = request.CustomerId, 
                CreatedAt = request.CreatedAt,
                UpdatedAt = request.UpdatedAt,
                Price = request.Price,
                ProductId = request.ProductId,
                Quantity = request.Quantity,
                Status = request.Status
            };

            // If model is invalid, it will thow exception
            ValidatorTool.FluentValidate(_validator, entity);
            return Task.FromResult(_orderDal.Add(entity));
        }
    }
}
