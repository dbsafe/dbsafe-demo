using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace ProductDAL.PG.Db
{
    public class ProductEntities : DbContext
    {
        public ProductEntities(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
    }

    [Table("category", Schema = "public")]
    public class Category
    {
        [Column("id")]
        [Key]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }

    [Table("product", Schema = "public")]
    public class Product
    {
        [Column("id")]
        [Key]
        public int Id { get; set; }

        [Column("code")]
        public string Code { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("cost")]
        public decimal? Cost { get; set; }

        [Column("list_price")]
        public decimal? ListPrice { get; set; }

        [Column("category_id")]
        public int CategoryId { get; set; }

        [Column("supplier_id")]
        public int SupplierId { get; set; }

        [Column("release_date")]
        public DateTime? ReleaseDate { get; set; }

        [Column("created_on")]
        public DateTime? CreatedOn { get; set; }

        public virtual Category Category { get; set; }

        public virtual Supplier Supplier { get; set; }
    }

    [Table("supplier", Schema = "public")]
    public class Supplier
    {
        [Column("id")]
        [Key]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("contact_name")]
        public string ContactName { get; set; }

        [Column("contact_phone")]
        public string ContactPhone { get; set; }

        [Column("contact_email")]
        public string ContactEmail { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
