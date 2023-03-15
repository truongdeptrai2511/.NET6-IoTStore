using System.ComponentModel.DataAnnotations;

namespace IotSupplyStore.Models
{
    public class ProductOrder
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
