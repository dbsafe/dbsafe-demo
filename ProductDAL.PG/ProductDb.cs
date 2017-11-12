using System;
using System.Collections.Generic;
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
            throw new NotImplementedException();
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
    }
}
