using System;
using System.Collections.Generic;
using System.Text;
using Tesodev.Core.Entities;

namespace Tesodev.Core.SharedModels
{
    public class DeleteAddressResponseModel : IResponsable
    {
        public string Message { get; set; }

        public int Id { get; set; }
    }
}
