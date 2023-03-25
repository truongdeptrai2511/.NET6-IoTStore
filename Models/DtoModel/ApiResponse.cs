using Newtonsoft.Json.Serialization;
using System.Net;

namespace IotSupplyStore.Models.DtoModel
{
    public class ApiResponse
    {
        public ApiResponse()
        {
            ErrorMessages = new List<string>();
        }

        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
        public object Result { get; set; } = null;
        public List<string> ErrorMessages { get; set; }
    }
}
