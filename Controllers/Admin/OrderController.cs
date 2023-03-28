using IotSupplyStore.DataAccess;
using IotSupplyStore.Models.DtoModel;
using IotSupplyStore.Models.ViewModel;
using IotSupplyStore.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace IotSupplyStore.Controllers.Admin
{
    [ApiController]
    [Route("api/order")]
    //[Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public ApiResponse _response;
        public OrderController(IUnitOfWork db)
        {
            _unitOfWork = db;
            _response = new ApiResponse();
        }

        [ResponseCache(Duration = 60)]
        [HttpGet("get-all-order")]
        public async Task<IActionResult> GetAllOrders()
        {
            var OrdersList = await _unitOfWork.Order.GetAllAsync();

            if (OrdersList == null)
            {
                return new JsonResult(new ApiResponse()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "order not found",
                    Result = null,
                    ErrorMessages = null
                });
            }

            // Define List Order
            List<OrderVM> ListOrder = new List<OrderVM>();

            foreach (var order in OrdersList)
            {
                var DetailOrder = await _unitOfWork.ProductOrder.GetAllAsync(u => u.OrderId == order.Id);
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
        [ResponseCache(Duration = 60)]
        public async Task<IActionResult> GetDetailOrdersById(string orderId)
        {
            var order = await _unitOfWork.Order.GetFirstOrDefaultAsync(u => u.Id == orderId,false);
            if (order == null)
            {
                return new JsonResult(new ApiResponse()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "order not found",
                    Result = null
                });
            }

            var DetailOrder = await _unitOfWork.ProductOrder.GetAllAsync(u => u.OrderId == orderId);
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
        [ResponseCache(Duration = 60)]
        [HttpGet("get-order-by-user")]
        public async Task<IActionResult> GetOrdersByUser()
        {
            var UserId = User.Claims.FirstOrDefault(u => u.Type == "id").Value;
            var UserOrdersList = await _unitOfWork.Order.GetAllAsync(u => u.ApplicationUserId == UserId && u.PaymentStatus == false);

            // Define List Order
            List<OrderVM> ListOrder = new List<OrderVM>();

            foreach (var order in UserOrdersList)
            {
                var DetailOrder = await _unitOfWork.ProductOrder.GetAllAsync(u => u.OrderId == order.Id);
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
