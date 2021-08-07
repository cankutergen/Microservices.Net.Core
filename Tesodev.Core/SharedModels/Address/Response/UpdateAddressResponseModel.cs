using System;
using System.Collections.Generic;
using System.Text;
using Tesodev.Core.Entities;

namespace Tesodev.Core.SharedModels
{
    public class UpdateAddressResponseModel : IResponsable
    {
        public int Id { get; set; }

        public string Message { get; set; }
    }
}
