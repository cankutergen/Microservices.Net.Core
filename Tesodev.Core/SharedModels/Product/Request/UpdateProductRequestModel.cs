using System;
using System.Collections.Generic;
using System.Text;

namespace Tesodev.Core.SharedModels.Product.Request
{
    public class UpdateProductRequestModel
    {
        public int Id { get; set; }

        public string ImageUrl { get; set; }

        public string Name { get; set; }
    }
}
