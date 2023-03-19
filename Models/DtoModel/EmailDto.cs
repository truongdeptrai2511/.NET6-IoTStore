namespace IotSupplyStore.Models.DtoModel
{
    public class EmailDto
    {
        public string ToName { get; set; } = String.Empty;
        public string ToEmailAddress { get; set; } = String.Empty;
        public string Subject { get; set; } = String.Empty;
        public string Body { get; set; } = String.Empty;
    }
}
