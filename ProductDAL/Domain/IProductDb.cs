using System.Collections.Generic;

namespace ProductDAL.Domain
{
    public interface IProductDb
    {
        IList<Supplier> GetSuppliers();
        IList<Category> GetCategories();
        bool UpdateSupplier(Supplier supplier);

        IList<ProductSummary> GetProductSummaries();
        Product GetProduct(int productId);

        int AddProduct(Product product);
    }
}
