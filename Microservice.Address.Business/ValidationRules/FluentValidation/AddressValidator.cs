using FluentValidation;
using Microservice.Address.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Address.Business.ValidationRules.FluentValidation
{
    public class AddressValidator : AbstractValidator<AddressModel>
    {
        public AddressValidator()
        {
            RuleFor(x => x.City)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.Country)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.CityCode)
                .NotNull()
                .NotEqual(0);

            RuleFor(x => x.AddressLine)
                .NotNull()
                .NotEmpty();
        }
    }
}
