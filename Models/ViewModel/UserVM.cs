namespace IotSupplyStore.Models.ViewModel
{
    public class UserVM
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public List<string> Role { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public string Address { get; set; }
        public DateTime CreatedAt { get; set; }  = DateTime.UtcNow;
        public ICollection<Order> Orders { get; set; }
    }
}
