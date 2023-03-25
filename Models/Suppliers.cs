using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IotSupplyStore.Models
{
    public class Suppliers
    {
        [Key]
        public int Id { get; set; }
        
        [StringLength(50)]
        public string SupplierName { get; set; }
        [StringLength(50)]
        public string SupplierEmail { get; set; }
        [StringLength(20)]
        public string SupplierPhoneNumber { get; set; }
        [StringLength(50)]
        public string SupplierFax { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
