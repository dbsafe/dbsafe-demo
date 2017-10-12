using System.Collections.Generic;

namespace ProductDAL.Domain
{
    public interface IProductDb
    {
        IList<Supplier> GetSupliers();
        IList<Category> GetCategories();

        IList<ProductSummary> GetProductSummaries();
        Product GetProduct(int productId);

        int AddProduct(Product product);
    }
}
