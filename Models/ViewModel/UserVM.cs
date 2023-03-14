namespace IotSupplyStore.Models.ViewModel
{
    public class UserVM
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public string Address { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ICollection<Transactions> TransactionList { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
