using System;
using System.Collections.Generic;
using System.Text;
using Tesodev.Core.Entities;

namespace Tesodev.Core.SharedModels.Customer.Response
{
    public class GetCustomerResponseModel : IResponsable
    {
        public int Id { get; set; }

        public int AddressId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string Message { get; set; }
    }
}
