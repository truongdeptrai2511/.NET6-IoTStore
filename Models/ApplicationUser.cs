using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace IotSupplyStore.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public string Avatar { get; set; }
        public string Address { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public ICollection<Transactions> TransactionList { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<Category> Category { get; set; }
        public string citizenIdentification { get; set; } //REQUIRED
    }
}
