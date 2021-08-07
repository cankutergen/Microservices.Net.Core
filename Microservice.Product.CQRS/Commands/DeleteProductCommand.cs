using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Product.CQRS.Commands
{
    public class DeleteProductCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public DeleteProductCommand()
        {

        }

        public DeleteProductCommand(int id)
        {
            Id = id;
        }
    }
}
