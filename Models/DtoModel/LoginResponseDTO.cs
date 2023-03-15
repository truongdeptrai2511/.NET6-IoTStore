namespace IotSupplyStore.Models.DtoModel
{
    public class LoginResponseDTO
    {
        public string Token { get; set; }
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Avatar { set; get; }
        public string Email { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string PhoneNumber { get; set; }
        public string citizenIdentification { get; set; }
    }
}
