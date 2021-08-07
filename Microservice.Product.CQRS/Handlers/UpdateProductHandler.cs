using FluentValidation;
using MediatR;
using Microservice.Product.CQRS.Commands;
using Microservice.Product.CQRS.Queries;
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
    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, ProductModel>
    {
        private readonly IProductDal _productDal;
        private readonly IValidator<ProductModel> _validator;
        private readonly IMediator _mediator;

        public UpdateProductHandler(IProductDal productDal, IValidator<ProductModel> validator, IMediator mediator)
        {
            _productDal = productDal;
            _validator = validator;
            _mediator = mediator;
        }

        public async Task<ProductModel> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var entity = await _mediator.Send(new GetProductByIdQuery(request.Id));

            // Entity must be exist
            if (entity == null)
            {
                throw new Exception($"Product with id:{request.Id} does not found");
            }

            var model = new ProductModel
            {
                Id = request.Id,
                ImageUrl = request.ImageUrl,
                Name = request.Name
            };

            // If model is invalid, it will thow exception
            ValidatorTool.FluentValidate(_validator, model);
            return await Task.FromResult(_productDal.Update(model));
        }
    }
}
