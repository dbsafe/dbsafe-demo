using DbSafe;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProductBL.Domain;
using PgDbSafe;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductDAL.PG.Tests
{
    [TestClass]
    public class ProductDbTest
    {
        private ProductDb _target;
        private IDbSafeManager _dbSafe;

        private Category _category1 = new Category { Id = 1, Name = "category-1" };
        private Category _category2 = new Category { Id = 2, Name = "category-2" };
        private Category _category3 = new Category { Id = 3, Name = "category-3" };

        private ProductSummary _productSummary1 = new ProductSummary { Id = 1, Code = "code-1", Name = "product-1", Category = "category-1", Supplier = "supplier-1" };
        private ProductSummary _productSummary2 = new ProductSummary { Id = 2, Code = "code-2", Name = "product-2", Category = "category-1", Supplier = "supplier-2" };
        private ProductSummary _productSummary3 = new ProductSummary { Id = 3, Code = "code-3", Name = "product-3", Category = "category-2", Supplier = "supplier-1" };

        private Product _product2 = new Product { Id = 2, Code = "code-2", Name = "product-2", CategoryId = 1, SupplierId = 2, Cost = 102.10m, Description = "desc-2", ListPrice = 112.10m, ReleaseDate = new DateTime(2000, 1, 2), CreatedOn = new DateTime(2000, 1, 1, 10, 11, 12) };
        private Product _product100 = new Product { Code = "code-100", Name = "product-100", CategoryId = 1, SupplierId = 2, Cost = 1000m, Description = "desc-100", ListPrice = 1100m, ReleaseDate = new DateTime(2010, 10, 10), CreatedOn = new DateTime(2010, 1, 1, 12, 12, 12) };

        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            _target = new ProductDb(new FakeTimeService());
            _target.Log = TestContext.WriteLine;

            _dbSafe = PgDbSafeManager.Initialize("product-db-test.xml")
                .SetConnectionString("ProductEntities-Test-Framework")
                .ExecuteScripts("delete-products", "delete-categories", "delete-suppliers", "reseed-product-table")
                .LoadTables("categories", "suppliers", "products")

                .RegisterFormatter(typeof(DateTime), new DateTimeFormatter("yyyy-MM-dd HH:mm:ss"))
                .RegisterFormatter("release_date", new DateTimeFormatter("yyyy-MM-dd"))
                .RegisterFormatter(typeof(decimal), new DecimalFormatter("0.00"));

            Console.WriteLine($"IsGlobalConfig: {_dbSafe.Config.IsGlobalConfig}, SerializeTests: {_dbSafe.Config.SerializeTests}");
        }

        [TestCleanup]
        public void Cleanup()
        {
            _dbSafe?.Completed();
        }

        [TestMethod]
        public void GetCategories_Must_return_all_the_categories()
        {
            var expected = new List<Category>() { _category1, _category2, _category3 };

            var actual = _target.GetCategories();

            AssertCategories(expected, actual);
        }

        [TestMethod]
        public void GetProductSummaries_Must_return_all_the_product_summaries()
        {
            var expected = new List<ProductSummary>() { _productSummary1, _productSummary2, _productSummary3 };

            var actual = _target.GetProductSummaries();

            AssertProductSummaries(expected, actual);
        }

        [TestMethod]
        public void GetProduct_Given_product_id_Must_return_correct_product()
        {
            var actual = _target.GetProduct(2);

            AssertProduct(_product2, actual);
        }

        [TestMethod]
        public void GetProduct_Given_not_found_product_id_Must_return_null()
        {
            var actual = _target.GetProduct(1000);

            Assert.IsNull(actual);
        }

        [TestMethod]
        public void AddProduct_Given_product_Must_insert_new_record()
        {
            _target.AddProduct(_product100);

            _dbSafe.AssertDatasetVsScript("products-after-insert", "select-all-products", "id");
        }

        [TestMethod]
        public void AddProduct_Given_product_Must_return_id_of_new_record()
        {
            var actual = _target.AddProduct(_product100);

            Assert.AreEqual(100, actual);
        }

        [TestMethod]
        public void UpdateSupplier_Given_supplier_Must_update_record_and_return_true()
        {
            var supplier2 = new Supplier
            {
                Id = 2,
                Name = "supplier-2-updated",
                ContactName = "contact-name-2-updated",
                ContactPhone = "100-200-9999",
                ContactEmail = "email-2-updated@test.com"
            };

            var actual = _target.UpdateSupplier(supplier2);

            Assert.IsTrue(actual);
            _dbSafe.AssertDatasetVsScript("suppliers-updated", "select-all-suppliers", "id");
        }

        [TestMethod]
        public void UpdateSupplier_Given_an_id_that_does_not_exist_Must_return_false_whiout_updating_any_record()
        {
            var supplier2 = new Supplier
            {
                Id = 200,
                Name = "supplier-2-updated",
                ContactName = "contact-name-2-updated",
                ContactPhone = "100-200-9999",
                ContactEmail = "email-2-updated@test.com"
            };

            var actual = _target.UpdateSupplier(supplier2);

            Assert.IsFalse(actual);
            _dbSafe.AssertDatasetVsScript("suppliers", "select-all-suppliers", "id");
        }

        private void AssertCategories(IList<Category> expected, IList<Category> actual)
        {
            Assert.AreEqual(expected.Count, actual.Count);

            foreach (var expectedCategory in expected)
            {
                var actualCategory = actual.FirstOrDefault(a => a.Id == expectedCategory.Id);
                Assert.IsNotNull(actualCategory, $"Category with Id '{expectedCategory.Id}' not found.");
                AssertCategory(expectedCategory, actualCategory);
            }
        }

        private void AssertCategory(Category expected, Category actual)
        {
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Name, actual.Name);
        }

        private void AssertProductSummaries(IList<ProductSummary> expected, IList<ProductSummary> actual)
        {
            Assert.AreEqual(expected.Count, actual.Count);

            foreach (var expectedProductSummary in expected)
            {
                var actualSummary = actual.FirstOrDefault(a => a.Id == expectedProductSummary.Id);
                Assert.IsNotNull(actualSummary, $"Category with Id '{expectedProductSummary.Id}' not found.");
                AssertProductSummary(expectedProductSummary, actualSummary);
            }
        }

        private void AssertProductSummary(ProductSummary expected, ProductSummary actual)
        {
            Assert.AreEqual(expected.Category, actual.Category);
            Assert.AreEqual(expected.Code, actual.Code);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Supplier, actual.Supplier);
        }

        private void AssertProduct(Product expected, Product actual)
        {
            Assert.AreEqual(expected.CategoryId, actual.CategoryId);
            Assert.AreEqual(expected.Code, actual.Code);
            Assert.AreEqual(expected.Cost, actual.Cost);
            Assert.AreEqual(expected.Description, actual.Description);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.ListPrice, actual.ListPrice);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.SupplierId, actual.SupplierId);
            Assert.AreEqual(expected.ReleaseDate, actual.ReleaseDate);
            Assert.AreEqual(expected.CreatedOn, actual.CreatedOn);
        }

        private string FormatDateTime(DateTime value)
        {
            if (value.TimeOfDay == TimeSpan.Zero)
            {
                return value.ToString("yyyy-MM-dd");
            }
            else
            {
                return value.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        private string FormatDateTimeWithMs(DateTime value)
        {
            if (value.TimeOfDay == TimeSpan.Zero)
            {
                return value.ToString("yyyy-MM-dd");
            }

            if (value.TimeOfDay.Milliseconds == 0)
            {
                return value.ToString("yyyy-MM-dd HH:mm:ss");
            }

            return value.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }

        private class FakeTimeService : ITimeService
        {
            public DateTime Now => new DateTime(2015, 10, 20, 14, 50, 01, 456);
        }
    }
}
