using IotSupplyStore.DataAccess;
using IotSupplyStore.Models;
using IotSupplyStore.Models.DtoModel;
using IotSupplyStore.Models.UpsertModel;
using IotSupplyStore.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace IotSupplyStore.Controllers.Customer
{
    [ApiController]
    [Authorize]
    [Route("api/order")]
    public class CartController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public ApiResponse _response;
        public CartController(ApplicationDbContext db)
        {
            _db = db;
            _response = new ApiResponse();
        }

        [HttpGet]
        [ResponseCache(Duration = 60)]
        public async Task<IActionResult> GetOrder()
        {
            // Get User ID from Claims
            var UserId = User.Claims.FirstOrDefault(u => u.Type == "id").Value;

            // Get list orders of user having payment's pending
            var UserOrder = await _db.Orders.Where(u => u.ApplicationUserId == UserId && u.PaymentStatus == false).ToListAsync();

            // Define List Order
            List<OrderVM> ListOrder = new List<OrderVM>();

            foreach (var order in UserOrder)
            {
                var DetailOrder = await _db.ProductOrders.Where(u => u.OrderId == order.Id).ToListAsync();
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
            var Customer = await _db.User.AsNoTracking().FirstOrDefaultAsync(User => User.Id == CustomerId);

            var ProductOrdersList = new List<ProductOrder>();
            var ProductEnitity = await _db.Products.ToListAsync();

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

            var NewOrder = await _db.Orders.AddAsync(new Order()
            {
                Id = NewOrderId,
                ApplicationUserId = CustomerId,
                CustomerName = Customer.FullName,
                PhoneNumber = Customer.PhoneNumber,
                Address = Customer.Address,
                OrderTotal = OrderTotal
            });

            _db.ProductOrders.AddRange(ProductOrdersList);
            await _db.SaveChangesAsync();

            _response.StatusCode = HttpStatusCode.OK;
            _response.Message = "Added successfully";
            _response.Result = ProductOrdersList;
            _response.ErrorMessages = null;
            return new JsonResult(_response);
        }
    }
}
