using FluentValidation;
using MediatR;
using Microservice.Address.CQRS.Commands;
using Microservice.Address.CQRS.Queries;
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
    public class UpdateAddressHandler : IRequestHandler<UpdateAddressCommand, AddressModel>
    {
        private readonly IAddressDal _addressDal;
        private readonly IValidator<AddressModel> _validator;
        private readonly IMediator _mediator;

        public UpdateAddressHandler(IAddressDal addressDal, IValidator<AddressModel> validator, IMediator mediator)
        {
            _addressDal = addressDal;
            _validator = validator;
            _mediator = mediator;
        }

        public async Task<AddressModel> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
        {
            var entity = await _mediator.Send(new GetAddresByIdQuery(request.Id));

            // Entity must be exist
            if (entity == null)
            {
                throw new Exception($"Address with id:{request.Id} does not found");
            }

            var model = new AddressModel
            {
                Id = request.Id,
                AddressLine = request.AddressLine,
                City = request.City,
                CityCode = request.CityCode,
                Country = request.Country
            };

            // If model is invalid, it will thow exception
            ValidatorTool.FluentValidate(_validator, model);
            return await Task.FromResult(_addressDal.Update(model));
        }
    }
}
