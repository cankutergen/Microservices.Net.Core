using System;
using System.Collections.Generic;
using System.Text;
using Tesodev.Core.Entities;

namespace Microservice.Address.Entities.Concrete
{
    public class AddressModel : IEntity
    {
        public int Id { get; set; }

        public string AddressLine { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public int CityCode { get; set; }
    }
}
