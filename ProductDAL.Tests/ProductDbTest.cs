﻿using DbSafe;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProductDAL.Domain;
using SqlDbSafe;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductDAL.Tests
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

        [TestInitialize]
        public void Initialize()
        {
            _target = new ProductDb();
            
            _dbSafe = SqlDbSafeManager.Initialize("product-db-test.xml")
                .SetConnectionString("ProductEntities-Test-Framework")
                .ExecuteScripts("delete-products", "delete-categories", "delete-suppliers", "reseed-product-table")
                .LoadTables("categories", "suppliers", "products")
                .RegisterFormatter(typeof(DateTime), value => FormatDateTime((DateTime)value))
                .RegisterFormatter("CreatedOn", value => FormatDateTimeWithMs((DateTime)value))
                .RegisterFormatter(typeof(decimal), value => ((decimal)value).ToString("0.00"));

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
        public void GetProductSummaries_Must_return_all_the_product_sumaries()
        {
            var expected = new List<ProductSummary>() { _productSummary1, _productSummary2, _productSummary3 };

            var actual = _target.GetProductSummaries();

            AssertProductSumaries(expected, actual);
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
            _dbSafe.ExecuteScripts("mock_get_date");

            _target.AddProduct(_product100);

            _dbSafe.AssertDatasetVsScript("products-after-insert", "select-all-products", "Id");
        }

        [TestMethod]
        public void AddProduct_Given_product_with_an_invalid_category_Must_raise_an_exception()
        {
            _product100.CategoryId = 100;
            try
            {
                _target.AddProduct(_product100);
                Assert.Fail("An exception was not raised.");
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Category not found", ex.InnerException.Message);
            }
        }

        [TestMethod]
        public void AddProduct_Given_product_Must_return_id_of_new_record()
        {
            var actual = _target.AddProduct(_product100);

            Assert.AreEqual(100, actual);
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

        private void AssertProductSumaries(IList<ProductSummary> expected, IList<ProductSummary> actual)
        {
            Assert.AreEqual(expected.Count, actual.Count);

            foreach (var expectedProductSumary in expected)
            {
                var actualSummary = actual.FirstOrDefault(a => a.Id == expectedProductSumary.Id);
                Assert.IsNotNull(actualSummary, $"Category with Id '{expectedProductSumary.Id}' not found.");
                AssertProductSumary(expectedProductSumary, actualSummary);
            }
        }

        private void AssertProductSumary(ProductSummary expected, ProductSummary actual)
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
    }
}