using MediatR;
using Microservice.Customer.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Customer.CQRS.Queries
{
    public class GetCustomerByIdQuery : IRequest<CustomerModel>
    {
        public int Id { get; set; }

        public GetCustomerByIdQuery()
        {

        }

        public GetCustomerByIdQuery(int id)
        {
            Id = id;
        }
    }
}
