using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Customer.CQRS.Commands
{
    public class DeleteCustomerCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public DeleteCustomerCommand()
        {

        }
        public DeleteCustomerCommand(int ıd)
        {
            Id = ıd;
        }
    }
}
