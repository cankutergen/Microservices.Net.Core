using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Order.CQRS.Commands
{
    public class UpdateOrderStatusCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public string Status { get; set; }

        public UpdateOrderStatusCommand()
        {

        }

        public UpdateOrderStatusCommand(int id, string status)
        {
            Id = id;
            Status = status;
        }
    }
}
