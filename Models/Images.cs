using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IotSupplyStore.Models
{
    public class Images
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ImName { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}
