using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IotSupplyStore.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace IotSupplyStore.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public int Or_Quantity { get; set; }
        public float Or_Price { get; set; }
        public float Or_PriceSale { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }
        public ICollection<Transactions> Transactions { get; set; }
        public ICollection<Product> Products { get; set; }
    }

}

