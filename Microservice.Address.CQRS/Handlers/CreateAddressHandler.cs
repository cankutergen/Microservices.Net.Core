using FluentValidation;
using MediatR;
using Microservice.Address.CQRS.Commands;
using Microservice.Address.DataAccess.Abstract;
using Microservice.Address.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tesodev.Core.CrossCuttingConcerns.FluentValidation;

namespace Microservice.Address.CQRS.Handlers
{
    public class CreateAddressHandler : IRequestHandler<CreateAddressCommand, AddressModel>
    {
        private readonly IAddressDal _addressDal;
        private readonly IValidator<AddressModel> _validator;

        public CreateAddressHandler(IAddressDal addressDal, IValidator<AddressModel> validator)
        {
            _addressDal = addressDal;
            _validator = validator;
        }

        public Task<AddressModel> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
        {
            var entity = new AddressModel
            {
                AddressLine = request.AddressLine,
                City = request.City,
                CityCode = request.CityCode,
                Country = request.Country
            };

            // If model is invalid, it will thow exception
            ValidatorTool.FluentValidate(_validator, entity);
            return Task.FromResult(_addressDal.Add(entity));

        }
    }
}
