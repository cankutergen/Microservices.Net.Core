using System;
using System.Collections.Generic;
using System.Text;

namespace Tesodev.Core.SharedModels
{
    public class CreateAddressRequestModel
    {
        public int Id { get; set; }

        public string AddressLine { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public int CityCode { get; set; }
    }
}
