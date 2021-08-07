using MediatR;
using Microservice.Address.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Address.CQRS.Queries
{
    public class GetAddresByIdQuery : IRequest<AddressModel>
    {
        public int Id { get; set; }

        public GetAddresByIdQuery()
        {

        }
        public GetAddresByIdQuery(int ıd)
        {
            Id = ıd;
        }
    }
}
