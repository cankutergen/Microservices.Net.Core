using MediatR;
using Microservice.Product.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Product.CQRS.Commands
{
    public class CreateProductCommand : IRequest<ProductModel>
    {
        public string ImageUrl { get; set; }

        public string Name { get; set; }

        public CreateProductCommand()
        {

        }
        public CreateProductCommand(string ımageUrl, string name)
        {
            ImageUrl = ımageUrl;
            Name = name;
        }
    }
}
