using Microservice.Order.DataAccess.Abstract;
using Microservice.Order.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using Tesodev.Core.DataAccess.EntityFramework;

namespace Microservice.Order.DataAccess.Concrete.EntityFramework
{
    public class EfOrderDal : EfEntityRepositoryBase<OrderModel, OrderContext>, IOrderDal
    { 
    }
}
