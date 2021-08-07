using MediatR;
using Microservice.Customer.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Customer.CQRS.Commands
{
    public class CreateCustomerCommand : IRequest<CustomerModel>
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public int AddressId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public CreateCustomerCommand()
        {

        }

        public CreateCustomerCommand(string name, string email, int addressId, DateTime createdAt, DateTime updatedAt)
        {
            Name = name;
            Email = email;
            AddressId = addressId;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }
    }
}
