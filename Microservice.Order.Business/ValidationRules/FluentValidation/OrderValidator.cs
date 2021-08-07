using FluentValidation;
using Microservice.Order.Entities.Concrete;

using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Order.Business.ValidationRules.FluentValidation
{
    public class OrderValidator : AbstractValidator<OrderModel>
    {
        public OrderValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotNull()
                .GreaterThan(0);

            RuleFor(x => x.Quantity)
                .NotNull()
                .GreaterThan(0);

            RuleFor(x => x.Price)
                .NotNull()
                .GreaterThan(0);

            RuleFor(x => x.Status)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.AddressId)
                .NotNull()
                .NotEqual(0);

            RuleFor(x => x.ProductId)
                .NotNull()
                .NotEqual(0);

            RuleFor(x => x.CreatedAt).NotNull();

            RuleFor(x => x.UpdatedAt).NotNull();
        }     
    }
}
