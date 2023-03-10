using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Bogus;
namespace IotSupplyStore.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int SupplierId { get; set; }
        public int UserId { get; set; }
        public string P_Code { get; set; }
        public string P_Status { get; set; }
        public int P_Quantity { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public ICollection<DetailProduct> DetailProductId { get; set; }
        [ForeignKey("SupplierId")]
        public Suppliers Suppliers { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
