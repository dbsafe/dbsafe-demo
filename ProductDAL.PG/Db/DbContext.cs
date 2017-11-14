using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductDAL.PG.Db
{
    public class ProductEntities : DbContext
    {
        public ProductEntities(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        public ProductEntities()
            : base("ProductEntities")
        {
        }

        public DbSet<Category> Categories { get; set; }
    }

    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
