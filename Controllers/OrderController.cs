using IotSupplyStore.DataAccess;
using IotSupplyStore.Models.DtoModel;
using IotSupplyStore.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace IotSupplyStore.Controllers
{
    [ApiController]
    [Route("api/order")]
    //[Authorize]
    public class OrderController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public ApiResponse _response;
        public OrderController(ApplicationDbContext db)
        {
            _db = db;
            _response = new ApiResponse();
        }


        [HttpGet("id")]
        public async Task<IActionResult> GetDetailForOrder(int orderId)
        {
            var order = await _db.Orders.FirstOrDefaultAsync(u => u.Id == orderId);
            if (order == null)
            {
                return new JsonResult(new ApiResponse()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "order not found",
                    Result = null
                });
            }

            var DetailOrder = await _db.ProductOrders.Where(u => u.Id == orderId).ToListAsync();
            OrderVM orderVM = new OrderVM()
            {
                Order = order,
                ProductOrders = DetailOrder
            };

            return new JsonResult(new ApiResponse()
            {
                StatusCode = HttpStatusCode.OK,
                Message = "success",
                Result = orderVM,
                ErrorMessages = null
            });
        }

        [Authorize]
        [HttpGet("get-order-by-user")]
        public async Task<IActionResult> GetOrderByUser()
        {
            var UserId = User.Claims.FirstOrDefault(u => u.Type == "id").Value;
            var UserOrder = await _db.Orders.LastOrDefaultAsync(u => u.ApplicationUserId == UserId);
            var DetailOrder = await _db.ProductOrders.Where(u => u.Id == UserOrder.Id).ToListAsync();

            OrderVM orderVM = new OrderVM()
            {
                Order = UserOrder,
                ProductOrders = DetailOrder
            };
            _response.Message = "success";
            _response.Result = orderVM;
            _response.StatusCode = HttpStatusCode.OK;
            _response.ErrorMessages = null;
            return new JsonResult(_response);
        }
    }
}
