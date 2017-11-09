using Domain = ProductBL.Domain;
using ProductDAL.Db;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace ProductDAL
{
    public class ProductDb : Domain.IProductDb
    {
        public int AddProduct(Domain.Product product)
        {
            using (var db = CreateDbContext())
            {
                var parameterId = new ObjectParameter("Id", typeof(int));
                db.usp_AddProduct(
                    product.Code,
                    product.Name,
                    product.Description,
                    product.Cost,
                    product.ListPrice,
                    product.CategoryId,
                    product.SupplierId,
                    product.ReleaseDate,
                    parameterId);
                return (int)parameterId.Value;
            }
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
            return new ProductEntities();
        }
    }
}
