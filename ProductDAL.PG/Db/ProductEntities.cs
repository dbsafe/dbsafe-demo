using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;

namespace ProductDAL.PG.Db
{
    public class ProductEntities : DbContext
    {
        static ProductEntities()
        {
            // Avoid ET trying to migrate the DB.
            Database.SetInitializer<ProductEntities>(null);
        }

        public ProductEntities(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {            
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }

        // TODO: continue here
        public virtual int UspAddProduct(string code, string name, string description, Nullable<decimal> cost, Nullable<decimal> listPrice, Nullable<int> categoryId, Nullable<int> supplierId, Nullable<System.DateTime> releaseDate, ObjectParameter id)
        {
            var codeParameter = code != null ?
                new ObjectParameter("Code", code) :
                new ObjectParameter("Code", typeof(string));

            var nameParameter = name != null ?
                new ObjectParameter("Name", name) :
                new ObjectParameter("Name", typeof(string));

            var descriptionParameter = description != null ?
                new ObjectParameter("Description", description) :
                new ObjectParameter("Description", typeof(string));

            var costParameter = cost.HasValue ?
                new ObjectParameter("Cost", cost) :
                new ObjectParameter("Cost", typeof(decimal));

            var listPriceParameter = listPrice.HasValue ?
                new ObjectParameter("ListPrice", listPrice) :
                new ObjectParameter("ListPrice", typeof(decimal));

            var categoryIdParameter = categoryId.HasValue ?
                new ObjectParameter("CategoryId", categoryId) :
                new ObjectParameter("CategoryId", typeof(int));

            var supplierIdParameter = supplierId.HasValue ?
                new ObjectParameter("SupplierId", supplierId) :
                new ObjectParameter("SupplierId", typeof(int));

            var releaseDateParameter = releaseDate.HasValue ?
                new ObjectParameter("ReleaseDate", releaseDate) :
                new ObjectParameter("ReleaseDate", typeof(System.DateTime));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("usp_AddProduct", codeParameter, nameParameter, descriptionParameter, costParameter, listPriceParameter, categoryIdParameter, supplierIdParameter, releaseDateParameter, id);
        }
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
