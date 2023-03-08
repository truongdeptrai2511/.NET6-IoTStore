using System.ComponentModel.DataAnnotations;

namespace IotSupplyStore.Models
{
    public class AdminUser
    {
        [Key]
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Avatar { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string status { get; set; }
        public ICollection<Suppliers> Suppliers { get; set; }
        public ICollection<Category> Category { get; set; }
    }
}
