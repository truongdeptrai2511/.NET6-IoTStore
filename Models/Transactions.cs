using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IotSupplyStore.Models
{
    public class Transactions
    {
        [Key]
        public int Id { get; set; }

        public int Tr_Total { get; set; }
        [StringLength(200)]
        public string Tr_Note { get; set; }
        [StringLength(200)]
        public string Tr_Address { get; set; }
        [StringLength(20)]
        public string Tr_Phone { get; set; }
        [StringLength(50)]
        public string Tr_Payment { get; set; }
        [StringLength(50)]
        public string Tr_Status { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [ForeignKey("ApplicationUserId")]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        
        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public Order Order { get; set; }
    }
}
