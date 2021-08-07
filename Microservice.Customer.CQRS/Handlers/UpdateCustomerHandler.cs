using FluentValidation;
using MediatR;
using Microservice.Customer.CQRS.Commands;
using Microservice.Customer.CQRS.Queries;
using Microservice.Customer.DataAccess.Abstract;
using Microservice.Customer.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tesodev.Core.CrossCuttingConcerns.FluentValidation;

namespace Microservice.Customer.CQRS.Handlers
{
    public class UpdateCustomerHandler : IRequestHandler<UpdateCustomerCommand, bool>
    {
        private readonly ICustomerDal _customerDal;
        private readonly IMediator _mediator;
        private readonly IValidator<CustomerModel> _validator;

        public UpdateCustomerHandler(ICustomerDal customerDal, IMediator mediator, IValidator<CustomerModel> validator)
        {
            _customerDal = customerDal;
            _mediator = mediator;
            _validator = validator;
        }

        public async Task<bool> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var model = new CustomerModel
            {
                AddressId = request.AddressId,
                CreatedAt = request.CreatedAt,
                Email = request.Email,
                Id = request.Id,
                Name = request.Name,
                UpdatedAt = request.UpdatedAt
            };

            // Entity must be exist
            var entity = await _mediator.Send(new GetCustomerByIdQuery(request.Id));
            if(entity == null)
            {
                throw new Exception($"Customer with id:{request.Id} does not found");
            }

            // If model is invalid, it will thow exception
            ValidatorTool.FluentValidate(_validator, model);
            await Task.FromResult(_customerDal.Update(model));
            return true;
        }
    }
}
