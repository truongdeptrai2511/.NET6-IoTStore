using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace IotSupplyStore.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        public string ProductName { get; set; }
        public string ImgName { get; set; }
        [StringLength(100)]
        public string Code { get; set; }
        [StringLength(50)]
        public string Status { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
        public string Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public int SupplierId { get; set; }
        [ForeignKey("SupplierId")]
        public Suppliers Suppliers { get; set; }

        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
    }
}

