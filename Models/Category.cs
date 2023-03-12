using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IotSupplyStore.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        public string C_Name { get; set; }
        [StringLength(50)]
        public string C_Home { get; set; }
        [StringLength(100)]
        public string C_Icon { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
 