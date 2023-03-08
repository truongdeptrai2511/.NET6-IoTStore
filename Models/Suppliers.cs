using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IotSupplyStore.Models
{
    public class Suppliers
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string S_Name { get; set; }
        public string S_Email { get; set; }
        public string S_Phone { get; set; }
        public string S_Fax { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public ICollection<Product> ProductList { get; set; }
        [ForeignKey("UserId")]
        public AdminUser AdminUser { get; set; }
    }
}
