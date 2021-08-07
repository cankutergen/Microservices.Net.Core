using Microservice.Customer.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using Tesodev.Core.DataAccess;

namespace Microservice.Customer.DataAccess.Abstract
{
    public interface ICustomerDal : IEntityRepository<CustomerModel>
    {
    }
}
