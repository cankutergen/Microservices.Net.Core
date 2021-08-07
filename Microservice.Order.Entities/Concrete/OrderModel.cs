

using System;
using System.Collections.Generic;
using System.Text;
using Tesodev.Core.Entities;

namespace Microservice.Order.Entities.Concrete
{
    public class OrderModel : IEntity
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
    }
}
