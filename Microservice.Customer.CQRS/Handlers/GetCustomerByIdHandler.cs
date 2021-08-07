using MediatR;
using Microservice.Customer.CQRS.Queries;
using Microservice.Customer.DataAccess.Abstract;
using Microservice.Customer.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microservice.Customer.CQRS.Handlers
{
    public class GetCustomerByIdHandler : IRequestHandler<GetCustomerByIdQuery, CustomerModel>
    {
        private readonly ICustomerDal _customerDal;

        public GetCustomerByIdHandler(ICustomerDal customerDal)
        {
            _customerDal = customerDal;
        }

        public Task<CustomerModel> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_customerDal.Get(x => x.Id == request.Id));
        }
    }
}
