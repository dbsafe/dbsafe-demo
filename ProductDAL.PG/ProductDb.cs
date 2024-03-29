﻿using ProductDAL.PG.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using Domain = ProductBL.Domain;

namespace ProductDAL.PG
{
    public interface ITimeService
    {
        DateTime Now { get; }
    }

    public class ProductDb : Domain.IProductDb
    {
        private readonly ITimeService _timeService;

        public string NameOrConnectionString { get; set; } = "ProductEntities";

        public Action<string> Log { get; set; }

        public ProductDb(ITimeService timeService)
        {
            _timeService = timeService;
        }

        public int AddProduct(Domain.Product product)
        {
            var productRecord = new Product
            {
                CategoryId = product.CategoryId,
                Code = product.Code,
                Cost = product.Cost,
                CreatedOn = _timeService.Now,
                Description = product.Description,
                ListPrice = product.ListPrice,
                Name = product.Name,
                ReleaseDate = product.ReleaseDate,
                SupplierId = product.SupplierId
            };

            using (var db = CreateDbContext())
            {

                db.Products.Add(productRecord);
                db.SaveChanges();
            }

            return productRecord.Id;
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
            using (var db = CreateDbContext())
            {
                var record = db.Products.FirstOrDefault(a => a.Id == productId);
                if (record == null)
                {
                    return null;
                }

                return new Domain.Product
                {
                    CategoryId = record.CategoryId,
                    Code = record.Code,
                    Description = record.Description,
                    Cost = record.Cost,
                    Id = record.Id,
                    ListPrice = record.ListPrice,
                    Name = record.Name,
                    SupplierId = record.SupplierId,
                    ReleaseDate = record.ReleaseDate,
                    CreatedOn = record.CreatedOn
                };
            }
        }

        public IList<Domain.ProductSummary> GetProductSummaries()
        {
            using (var db = CreateDbContext())
            {
                var list = db.Products
                    .Select(record => new
                    {
                        record.Id,
                        record.Code,
                        record.Name,
                        CategoryName = record.Category.Name,
                        SupplierName = record.Supplier.Name
                    }).ToList();

                return list.Select(a => new Domain.ProductSummary
                {
                    Id = a.Id,
                    Code = a.Code,
                    Name = a.Name,
                    Category = a.CategoryName,
                    Supplier = a.SupplierName
                }).ToList();
            }
        }

        public IList<Domain.Supplier> GetSuppliers()
        {
            throw new NotImplementedException();
        }

        public bool UpdateSupplier(Domain.Supplier supplier)
        {
            using (var db = CreateDbContext())
            {
                var record = db.Suppliers.FirstOrDefault(a => a.Id == supplier.Id);
                if (record == null)
                {
                    return false;
                }

                record.ContactEmail = supplier.ContactEmail;
                record.ContactName = supplier.ContactName;
                record.ContactPhone = supplier.ContactPhone;
                record.Name = supplier.Name;
                record.ContactEmail = supplier.ContactEmail;
                db.SaveChanges();
                return true;
            }
        }

        private ProductEntities CreateDbContext()
        {
            var db = new ProductEntities(NameOrConnectionString);
            db.Database.Log = Log;
            return db;
        }
    }
}
