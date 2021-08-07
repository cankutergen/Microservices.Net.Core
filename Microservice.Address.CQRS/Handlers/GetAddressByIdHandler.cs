using MediatR;
using Microservice.Address.CQRS.Queries;
using Microservice.Address.DataAccess.Abstract;
using Microservice.Address.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microservice.Address.CQRS.Handlers
{
    public class GetAddressByIdHandler : IRequestHandler<GetAddresByIdQuery, AddressModel>
    {
        private readonly IAddressDal _addressDal;

        public GetAddressByIdHandler(IAddressDal addressDal)
        {
            _addressDal = addressDal;
        }

        public Task<AddressModel> Handle(GetAddresByIdQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_addressDal.Get(x => x.Id == request.Id));
        }
    }
}
