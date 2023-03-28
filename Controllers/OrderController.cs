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

        [HttpGet("get-all-order")]
        public async Task<IActionResult> GetAllOrders()
        {
            var OrdersList = await _db.Orders.AsNoTracking().ToListAsync();

            if (OrdersList == null)
            {
                return new JsonResult(new ApiResponse()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "order not found",
                    Result = null
                });
            }

            // Define List Order
            List<OrderVM> ListOrder = new List<OrderVM>();

            foreach (var order in OrdersList)
            {
                var DetailOrder = await _db.ProductOrders.Where(u => u.OrderId == order.Id).ToListAsync();
                ListOrder.Add(new OrderVM()
                {
                    Order = order,
                    ProductOrders = DetailOrder
                });
            }

            return new JsonResult(new ApiResponse()
            {
                StatusCode = HttpStatusCode.OK,
                Message = "success",
                Result = ListOrder,
                ErrorMessages = null
            });
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetDetailOrdersById(string orderId)
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

            var DetailOrder = await _db.ProductOrders.Where(u => u.OrderId == orderId).ToListAsync();
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
        public async Task<IActionResult> GetOrdersByUser()
        {
            var UserId = User.Claims.FirstOrDefault(u => u.Type == "id").Value;
            var UserOrdersList = await _db.Orders.Where(u => u.ApplicationUserId == UserId && u.PaymentStatus == false).ToListAsync();

            // Define List Order
            List<OrderVM> ListOrder = new List<OrderVM>();

            foreach (var order in UserOrdersList)
            {
                var DetailOrder = await _db.ProductOrders.AsNoTracking().Where(u => u.OrderId == order.Id).ToListAsync();
                ListOrder.Add(new OrderVM()
                {
                    Order = order,
                    ProductOrders = DetailOrder
                });
            }

            _response.Message = "success";
            _response.Result = ListOrder;
            _response.StatusCode = HttpStatusCode.OK;
            _response.ErrorMessages = null;
            return new JsonResult(_response);
        }
    }
}
