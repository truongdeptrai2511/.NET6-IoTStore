using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IotSupplyStore.Models
{
    public class Transactions
    {
        [Key]
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int OrderId { get; set; }
        public int Tr_Total { get; set; }
        public string Tr_Note { get; set; }
        public string Tr_Address { get; set; }
        public string Tr_Phone { get; set; }
        public string Tr_Payment { get; set; }
        public string Tr_Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }
        [ForeignKey("OrderId")]
        public Order Order { get; set; }
    }
}
