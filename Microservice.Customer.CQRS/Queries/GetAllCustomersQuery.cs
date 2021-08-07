using MediatR;
using Microservice.Customer.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Customer.CQRS.Queries
{
    public class GetAllCustomersQuery : IRequest<List<CustomerModel>>
    {

    }
}
