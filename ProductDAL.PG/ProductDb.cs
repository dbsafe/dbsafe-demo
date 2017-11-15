using Npgsql;
using ProductDAL.PG.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using Domain = ProductBL.Domain;

namespace ProductDAL.PG
{
    public class ProductDb : Domain.IProductDb
    {
        private string _nameOrConnectionString;

        public ProductDb(string nameOrConnectionString)
        {
            _nameOrConnectionString = nameOrConnectionString;
        }

        public ProductDb()
            :this("ProductEntities")
        {
        }

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
            var result = new List<Domain.ProductSummary>();
            var query = @"
SELECT p.id, p.name as p_name, p.code, c.name as category, s.name as supplier
FROM public.product p
JOIN public.category c ON c.id = p.category_id
JOIN public.supplier s ON s.id = p.supplier_id;
";
            ExecuteReader(query, (rdr) =>
            {
                while (rdr.Read())
                {
                    var productSummary = new Domain.ProductSummary
                    {
                        Id = (int)rdr["id"],
                        Name = rdr["p_name"].ToString(),
                        Category = rdr["category"].ToString(),
                        Code = rdr["code"].ToString(),
                        Supplier = rdr["supplier"].ToString(),
                    };

                    result.Add(productSummary);
                }
            });

            return result;
        }

        public IList<Domain.Supplier> GetSuppliers()
        {
            throw new NotImplementedException();
        }

        public bool UpdateSupplier(Domain.Supplier supplier)
        {
            throw new NotImplementedException();
        }

        private NpgsqlConnection CreateConnection()
        {
            var cnn = new NpgsqlConnection(_nameOrConnectionString);
            cnn.Open();
            return cnn;
        }

        private void ExecuteReader(string query, Action<NpgsqlDataReader> action)
        {
            using (var conn = CreateConnection())
            {
                using (var comm = new NpgsqlCommand(query, conn))
                {
                    var rdr = comm.ExecuteReader();
                    action(rdr);
                }
            }
        }

        private ProductEntities CreateDbContext()
        {
            return new ProductEntities(_nameOrConnectionString);
        }
    }
}
