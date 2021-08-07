using Microservice.Customer.DataAccess.Abstract;
using Microservice.Customer.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using Tesodev.Core.DataAccess.EntityFramework;

namespace Microservice.Customer.DataAccess.Concrete.EntityFramework
{
    public class EfCustomerDal : EfEntityRepositoryBase<CustomerModel, CustomerContext>, ICustomerDal
    {
    }
}
