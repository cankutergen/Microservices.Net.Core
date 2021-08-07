using FluentValidation;
using MediatR;
using Microservice.Product.CQRS.Commands;
using Microservice.Product.DataAccess.Abstract;
using Microservice.Product.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tesodev.Core.CrossCuttingConcerns.FluentValidation;

namespace Microservice.Product.CQRS.Handlers
{
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, ProductModel>
    {
        private readonly IProductDal _productDal;
        private readonly IValidator<ProductModel> _validator;

        public CreateProductHandler(IProductDal productDal, IValidator<ProductModel> validator)
        {
            _productDal = productDal;
            _validator = validator;
        }

        public Task<ProductModel> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var entity = new ProductModel
            {
                ImageUrl = request.ImageUrl,
                Name = request.Name
            };

            ValidatorTool.FluentValidate(_validator, entity);
            return Task.FromResult(_productDal.Add(entity));
        }
    }
}
