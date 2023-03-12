using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IotSupplyStore.Models
{
    public class Suppliers
    {
        [Key]
        public int Id { get; set; }
        
        [StringLength(50)]
        public string S_Name { get; set; }
        [StringLength(50)]
        public string S_Email { get; set; }
        [StringLength(20)]
        public string S_Phone { get; set; }
        [StringLength(50)]
        public string S_Fax { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        
        public ICollection<Product> Products { get; set; }
    }
}
