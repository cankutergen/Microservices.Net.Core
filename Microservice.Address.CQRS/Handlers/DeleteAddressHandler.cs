using MediatR;
using Microservice.Address.CQRS.Commands;
using Microservice.Address.CQRS.Queries;
using Microservice.Address.DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tesodev.Core.SharedModels;

namespace Microservice.Address.CQRS.Handlers
{
    public class DeleteAddressHandler : IRequestHandler<DeleteAddressCommand, bool>
    {
        private readonly IAddressDal _addressDal;
        private readonly IMediator _mediator;

        public DeleteAddressHandler(IAddressDal addressDal, IMediator mediator)
        {
            _addressDal = addressDal;
            _mediator = mediator;
        }

        public async Task<bool> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
        {
            // Entity must be exist
            var model = await _mediator.Send(new GetAddresByIdQuery(request.Id));
            if(model == null)
            {
                throw new Exception($"Address with id:{request.Id} does not found");
            }

            _addressDal.Delete(model);
            return true;
        }
    }
}
