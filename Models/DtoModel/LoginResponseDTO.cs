namespace IotSupplyStore.Models.DtoModel
{
    public class LoginResponseDTO
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
        public string FullName { get; set; }
        public string Avatar { set; get; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string PhoneNumber { get; set; }
        public string CitizenIdentification { get; set; }
    }
}
