using IotSupplyStore.Models;
using IotSupplyStore.Models.DtoModel;
using IotSupplyStore.Models.UpsertModel;
using IotSupplyStore.Models.ViewModel;
using IotSupplyStore.Repository.IRepository;
using IotSupplyStore.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace IotSupplyStore.Controllers.Customer
{
    [ApiController]
    [Authorize(Policy = SD.Policy_MakeOrder)]
    [Route("api/order")]
    public class CartController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public ApiResponse _response;
        public CartController(IUnitOfWork db)
        {
            _unitOfWork = db;
            _response = new ApiResponse();
        }

        [HttpGet]
        [ResponseCache(Duration = 60)]
        public async Task<IActionResult> GetOrder()
        {
            // Get ApplicationUsers ID from Claims
            var UserId = User.Claims.FirstOrDefault(u => u.Type == "id").Value;

            // Get list orders of user having payment's pending
            var UserOrder = await _unitOfWork.Order.GetAllAsync(u => u.ApplicationUserId == UserId && u.PaymentStatus == false);

            // Define List Order
            List<OrderVM> ListOrder = new List<OrderVM>();

            foreach (var order in UserOrder)
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

        [HttpPost]
        public async Task<IActionResult> MakeAnOrder(List<ShoppingCartUpsert> items)
        {
            var CustomerId = User.Claims.FirstOrDefault(u => u.Type == "id").Value;
            var Customer = await _unitOfWork.ApplicationUser.GetFirstOrDefaultAsync(User => User.Id == CustomerId,false);

            var ProductOrdersList = new List<ProductOrder>();
            var ProductEnitity = await _unitOfWork.Product.GetAllAsync();

            string NewOrderId = Guid.NewGuid().ToString();
            float OrderTotal = 0;

            foreach (var item in items)
            {
                var CheckQuantityFromRepository = ProductEnitity.FirstOrDefault(u => u.Id == item.ProductId).Quantity;

                if (item.Count > CheckQuantityFromRepository)
                {
                    _response.Message = $"Item with id {item.ProductId} it out of stock";
                    _response.Result = item;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages.Add($"Please re-check and make a other request again");
                    return new JsonResult(_response);
                }
                ProductOrdersList.Add(new ProductOrder
                {
                    Count = item.Count,
                    ProductId = item.ProductId,
                    Price = item.Price,
                    OrderId = NewOrderId
                });
                OrderTotal += item.Price;

                ProductEnitity.FirstOrDefault(u => u.Id == item.ProductId).Quantity = CheckQuantityFromRepository - item.Count;
            }

            await _unitOfWork.Order.Add(new Order()
            {
                Id = NewOrderId,
                ApplicationUserId = Customer.Id,
                CustomerName = Customer.FullName,
                PhoneNumber = Customer.PhoneNumber,
                Address = Customer.Address,
                OrderTotal = OrderTotal,
            });

            await _unitOfWork.ProductOrder.AddRange(ProductOrdersList);
            await _unitOfWork.Save();

            _response.StatusCode = HttpStatusCode.OK;
            _response.Message = "Added successfully";
            _response.Result = await _unitOfWork.Order.GetFirstOrDefaultAsync(u => u.Id == NewOrderId);
            _response.ErrorMessages = null;
            return new JsonResult(_response);
        }
    }
}
