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

        public float PriceSale { get; set; }
        public float OrderTotal { get; set; }
        public bool OrderStatus { get; set; } = false;
        public bool PaymentStatus { get; set; } = false;
        
        

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public DateTime ShippingDate { get; set; } = DateTime.Now.AddDays(2);
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public DateTime PaymentDate { get; set; }
        public DateTime PaymentDueDate { get; set; }


        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }


        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }
    }

}

