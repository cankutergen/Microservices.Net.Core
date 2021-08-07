using FluentValidation;
using Microservice.Product.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Product.Business.ValidationRules.FluentValidation
{
    public class ProductValidator : AbstractValidator<ProductModel>
    {
        public ProductValidator()
        {
            RuleFor(x => x.ImageUrl)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty();
        }
    }
}
