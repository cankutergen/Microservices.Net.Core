using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice.Order.Model
{
    public class OrderDetail
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }

        public string Status { get; set; }

        public Address Address { get; set; }

        public Product Product { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
