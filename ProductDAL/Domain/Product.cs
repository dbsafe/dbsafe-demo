using System;

namespace ProductDAL.Domain
{
    public class Product
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? Cost { get; set; }
        public decimal? ListPrice { get; set; }
        public int CategoryId { get; set; }
        public int SupplierId { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
