using IotSupplyStore.DataAccess;
using IotSupplyStore.Models.DtoModel;
using IotSupplyStore.Models.ViewModel;
using IotSupplyStore.Repository.IRepository;
using IotSupplyStore.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace IotSupplyStore.Controllers.Admin
{
    [ApiController]
    [Route("api/order")]
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

        [HttpDelete("delete-order/{productId}/{orderId}")]
        public async Task<IActionResult> DeleteOrder(string orderId, int productId)
        {
            // Lấy thông tin Order từ orderId
            var order = await _unitOfWork.Order.GetFirstOrDefaultAsync(u => u.Id == orderId);

            if (order == null)
            {
                return new JsonResult(new ApiResponse()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Order not found",
                    Result = null
                });
            }

            // Kiểm tra xem sản phẩm có tồn tại trong Order không
            var productOrder = await _unitOfWork.ProductOrder.GetFirstOrDefaultAsync(u => u.OrderId == orderId && u.ProductId == productId);

            if (productOrder == null)
            {
                return new JsonResult(new ApiResponse()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Product not found in the order",
                    Result = null
                });
            }

            // Xóa sản phẩm khỏi Order
            _unitOfWork.ProductOrder.Remove(productOrder);
            await _unitOfWork.Save();

            // Kiểm tra xem Order còn sản phẩm nào khác hay không
            var remainingProducts = await _unitOfWork.ProductOrder.GetAllAsync(u => u.OrderId == orderId);
            if (remainingProducts.Count() == 0)
            {
                // Nếu không còn sản phẩm nào trong Order, xóa cả Order
                _unitOfWork.Order.Remove(order);
                await _unitOfWork.Save();
            }

            return new JsonResult(new ApiResponse()
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Order successfully deleted",
                Result = null
            });
        }

    }
}
