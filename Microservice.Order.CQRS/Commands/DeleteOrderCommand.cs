using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Order.CQRS.Commands
{
    public class DeleteOrderCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public DeleteOrderCommand()
        {

        }

        public DeleteOrderCommand(int id)
        {
            Id = id;
        }
    }
}
