using System;
using System.Collections.Generic;
using System.Text;
using Tesodev.Core.Entities;

namespace Microservice.Product.Entities.Concrete
{
    public class ProductModel : IEntity
    {
        public int Id { get; set; }

        public string ImageUrl { get; set; }

        public string Name { get; set; }
    }
}
