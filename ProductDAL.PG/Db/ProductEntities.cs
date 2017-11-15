using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        public DbSet<Category> Categories { get; set; }
    }

    [Table("category", Schema = "public")]
    public class Category
    {
        [Column("id")]
        [Key]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }
    }
}
