using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IotSupplyStore.Models
{
    public class OrderStatus
    {
        [Key]
        public int Id { get; set; }
        public string ShipperId { get; set; }

        public bool StatusApproved { get; set; } = true;
        public bool StatusInProcess { get; set; } = false;
        public bool StatusShipped { get; set; } = false;
        public bool StatusCancelled { get; set; } = false;
        public bool StatusRefunded { get; set; } = false;

        public DateTime ShippingDateEstimate { get; set; } = DateTime.Now.AddDays(2);
        public DateTime ShippingDate { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public DateTime PaymentDate { get; set; }
        public DateTime PaymentDueDate { get; set; } = DateTime.Now.AddDays(7);

        public string OrderId { get; set; }
        [ForeignKey("OrderId")]
        public Order Order { get; set; }
    }
}
