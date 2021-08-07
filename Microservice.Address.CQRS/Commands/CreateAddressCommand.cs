using MediatR;
using Microservice.Address.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Address.CQRS.Commands
{
    public class CreateAddressCommand : IRequest<AddressModel>
    {
        public string AddressLine { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public int CityCode { get; set; }

        public CreateAddressCommand()
        {

        }
        public CreateAddressCommand(string addressLine, string city, string country, int cityCode)
        {
            AddressLine = addressLine;
            City = city;
            Country = country;
            CityCode = cityCode;
        }
    }
}
