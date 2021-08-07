using System;
using System.Collections.Generic;
using System.Text;

namespace Tesodev.Core.SharedModels.Product
{
    public class CreateProductRequestModel
    {
        public int Id { get; set; }

        public string ImageUrl { get; set; }

        public string Name { get; set; }
    }
}
