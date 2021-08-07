using Microservice.Address.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using Tesodev.Core.DataAccess;

namespace Microservice.Address.DataAccess.Abstract
{
    public interface IAddressDal : IEntityRepository<AddressModel>
    {
    }
}
