using Microservice.Address.DataAccess.Abstract;
using Microservice.Address.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using Tesodev.Core.DataAccess.EntityFramework;

namespace Microservice.Address.DataAccess.Concrete.EntityFramework
{
    public class EfAddressDal : EfEntityRepositoryBase<AddressModel, AddressContext>, IAddressDal
    {
    }
}
