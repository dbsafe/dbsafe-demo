using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using Domain = ProductBL.Domain;

namespace ProductDAL.PG
{
    public class ProductDb : Domain.IProductDb
    {
        private string _connectionString;

        public ProductDb(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int AddProduct(Domain.Product product)
        {
            throw new NotImplementedException();
        }

        public IList<Domain.Category> GetCategories()
        {
            var result = new List<Domain.Category>();
            var query = "SELECT * FROM public.category";
            ExecuteReader(query, (rdr) =>
            {
                while (rdr.Read())
                {
                    var category = new Domain.Category
                    {
                        Id = (int)rdr["id"],
                        Name = (string)rdr["name"]
                    };

                    result.Add(category);
                }
            });

            return result;
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
            var cnn = new NpgsqlConnection(_connectionString);
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
    }
}
