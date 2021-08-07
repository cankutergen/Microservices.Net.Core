using FluentValidation;
using Microservice.Customer.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Customer.Business.ValidationRules.FluentValidation
{
    public class CustomerValidator : AbstractValidator<CustomerModel>
    {


        public CustomerValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.Email)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.AddressId)
                .NotNull()
                .NotEqual(0);

            RuleFor(x => x.CreatedAt).NotNull();

            RuleFor(x => x.UpdatedAt).NotNull();
        }
    }
}
