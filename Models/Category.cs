using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IotSupplyStore.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string C_Name { get; set; }
        public string C_Home { get; set; }
        public string C_Nameoperty { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        [ForeignKey("UserId")]
        public AdminUser AdminUser { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
 