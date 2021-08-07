using Microservice.Order.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using Tesodev.Core.DataAccess;

namespace Microservice.Order.DataAccess.Abstract
{
    public interface IOrderDal : IEntityRepository<OrderModel>
    {
    }
}
