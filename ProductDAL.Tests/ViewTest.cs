using DbSafe;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlDbSafe;

namespace ProductDAL.Tests
{
    [TestClass]
    public class ViewTest
    {
        private IDbSafeManager _dbSafe;

        [TestInitialize]
        public void Initialize()
        {
            _dbSafe = SqlDbSafeManager.Initialize("product-db-test.xml")
                .SetConnectionString("ProductEntities-Test-Framework")
                .ExecuteScripts("delete-products", "delete-categories", "delete-suppliers", "reseed-product-table")
                .LoadTables("categories", "suppliers", "products");
        }

        [TestCleanup]
        public void Cleanup()
        {
            _dbSafe?.Completed();
        }

        [TestMethod]
        public void LoadingDataFromAViewTest()
        {
            _dbSafe.AssertDatasetVsScript("expected-products-using-view", "script-select-product-using-view", "Code");           
        }
    }
}
