using FluentValidation;
using MediatR;
using Microservice.Customer.CQRS.Commands;
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
    public class CreateCustomerHandler : IRequestHandler<CreateCustomerCommand, CustomerModel>
    {
        private readonly ICustomerDal _customerDal;
        private readonly IValidator<CustomerModel> _validator;

        public CreateCustomerHandler(ICustomerDal customerDal, IValidator<CustomerModel> validator)
        {
            _customerDal = customerDal;
            _validator = validator;
        }

        public Task<CustomerModel> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var entity = new CustomerModel { 
                AddressId = request.AddressId,     
                Email = request.Email, 
                Name = request.Name,
                CreatedAt = request.CreatedAt,
                UpdatedAt = request.UpdatedAt           
            };

            // If model is invalid, it will thow exception
            ValidatorTool.FluentValidate(_validator, entity);
            return Task.FromResult(_customerDal.Add(entity));
        }
    }
}
