using MediatR;
using Microservice.Address.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Address.CQRS.Commands
{
    public class UpdateAddressCommand : IRequest<AddressModel>
    {
        public int Id { get; set; }

        public string AddressLine { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public int CityCode { get; set; }

        public UpdateAddressCommand()
        {

        }
        public UpdateAddressCommand(int ıd, string addressLine, string city, string country, int cityCode)
        {
            Id = ıd;
            AddressLine = addressLine;
            City = city;
            Country = country;
            CityCode = cityCode;
        }
    }
}
