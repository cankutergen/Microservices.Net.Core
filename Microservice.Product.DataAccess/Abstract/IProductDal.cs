using Microservice.Product.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using Tesodev.Core.DataAccess;

namespace Microservice.Product.DataAccess.Abstract
{
    public interface IProductDal : IEntityRepository<ProductModel>
    {
    }
}
