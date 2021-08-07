using MediatR;
using Microservice.Product.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Product.CQRS.Commands
{
    public class UpdateProductCommand : IRequest<ProductModel>
    {
        public int Id { get; set; }

        public string ImageUrl { get; set; }

        public string Name { get; set; }

        public UpdateProductCommand()
        {

        }

        public UpdateProductCommand(int id, string imageUrl, string name)
        {
            Id = id;
            ImageUrl = imageUrl;
            Name = name;
        }
    }
}
