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
    public class GetAllCustomersHandler : IRequestHandler<GetAllCustomersQuery, List<CustomerModel>>
    {
        private readonly ICustomerDal _customerDal;

        public GetAllCustomersHandler(ICustomerDal customerDal)
        {
            _customerDal = customerDal;
        }

        public Task<List<CustomerModel>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_customerDal.GetList());
        }
    }
}
