using MediatR;
using Microservice.Order.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Order.CQRS.Commands
{
    public class CreateOrderCommand : IRequest<OrderModel>
    {
        public int Id { get; set; }

        public int AddressId { get; set; }

        public int ProductId { get; set; }

        public int CustomerId { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }

        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public CreateOrderCommand()
        {

        }

        public CreateOrderCommand(int id, int addressId, int productId, int customerId, int quantity, double price, string status, DateTime createdAt, DateTime updatedAt)
        {
            Id = id;
            AddressId = addressId;
            ProductId = productId;
            CustomerId = customerId;
            Quantity = quantity;
            Price = price;
            Status = status;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }
    }
}
