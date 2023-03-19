using IotSupplyStore.Models.DtoModel;

namespace IotSupplyStore.Service.IService
{
    public interface IEmailService
    {
        Task SendMail(EmailDto emailDto);
    }
}
