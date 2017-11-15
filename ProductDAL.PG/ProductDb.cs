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

            using (var conn = CreateConnection())
            {
                var query = "SELECT * FROM public.category";
                using (var comm = new NpgsqlCommand(query, conn))
                {
                    using (var rdr = comm.ExecuteReader())
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
                    }
                }
            }

            return result;
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

        private NpgsqlConnection CreateConnection()
        {
            var cnn = new NpgsqlConnection(_connectionString);
            cnn.Open();
            return cnn;
        }
    }
}
