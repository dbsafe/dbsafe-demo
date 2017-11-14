using ProductDAL.PG.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using Domain = ProductBL.Domain;

namespace ProductDAL.PG
{
    public class ProductDb : Domain.IProductDb
    {
        public int AddProduct(Domain.Product product)
        {
            throw new NotImplementedException();
        }

        public IList<Domain.Category> GetCategories()
        {
            using (var db = CreateDbContext())
            {
                return db.Categories
                    .ToList()
                    .Select(record => new Domain.Category { Id = record.Id, Name = record.Name })
                    .ToList();
            }
        }

        public Domain.Product GetProduct(int productId)
        {
            throw new NotImplementedException();
        }

        public IList<Domain.ProductSummary> GetProductSummaries()
        {
            throw new NotImplementedException();
        }

        public IList<Domain.Supplier> GetSuppliers()
        {
            throw new NotImplementedException();
        }

        public bool UpdateSupplier(Domain.Supplier supplier)
        {
            throw new NotImplementedException();
        }

        private ProductEntities CreateDbContext()
        {
            return new ProductEntities();
        }
    }
}
