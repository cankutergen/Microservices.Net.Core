using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Tesodev.Core.SharedModels;

namespace Microservice.Address.CQRS.Commands
{
    public class DeleteAddressCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public DeleteAddressCommand()
        {

        }

        public DeleteAddressCommand(int ıd)
        {
            Id = ıd;
        }
    }
}
