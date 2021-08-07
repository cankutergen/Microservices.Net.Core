using Microservice.Product.DataAccess.Abstract;
using Microservice.Product.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using Tesodev.Core.DataAccess.EntityFramework;

namespace Microservice.Product.DataAccess.Concrete.EntityFramework
{
    public class EfProductDal : EfEntityRepositoryBase<ProductModel, ProductContext>, IProductDal
    {
    }
}
