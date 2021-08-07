using MediatR;
using Microservice.Customer.CQRS.Commands;
using Microservice.Customer.CQRS.Queries;
using Microservice.Customer.DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microservice.Customer.CQRS.Handlers
{
    public class DeleteCustomerHandler : IRequestHandler<DeleteCustomerCommand, bool>
    {
        private readonly ICustomerDal _customerDal;
        private readonly IMediator _mediator;

        public DeleteCustomerHandler(ICustomerDal customerDal, IMediator mediator)
        {
            _customerDal = customerDal;
            _mediator = mediator;
        }

        public async Task<bool> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {

            // Entity must be exist
            var model = await _mediator.Send(new GetCustomerByIdQuery(request.Id));
            if(model == null)
            {
                throw new Exception($"Customer with id:{request.Id} does not found");
            }

            _customerDal.Delete(model);
            return true;
        }
    }
}
