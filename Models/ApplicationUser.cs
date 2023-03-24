using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace IotSupplyStore.Models
{
    public class ApplicationUser : IdentityUser
    {
        [StringLength(50)]
        public string FullName { get; set; }
        [StringLength(100)]
        public string Avatar { get; set; }
        [StringLength(200)]
        public string Address { get; set; }
        [StringLength(20)]
        public string citizenIdentification { get; set; } //REQUIRED
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        
        public ICollection<Order> Orders { get; set; }
        public ICollection<ShoppingCart> ShoppingCarts { get; set;}
    }
}
