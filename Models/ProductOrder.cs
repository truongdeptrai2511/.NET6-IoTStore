using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IotSupplyStore.Models
{
    public class ProductOrder
    {
        [Key] 
        public int Id { get; set; }

        public int Count { get; set; }
        public float Price { get; set; }

        public string OrderId { get; set; }
        [ForeignKey("OrderId")]
        public Order Order { get; set; }

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}
