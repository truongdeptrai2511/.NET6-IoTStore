using IotSupplyStore.DataAccess;
using IotSupplyStore.Models;
using IotSupplyStore.Models.DtoModel;
using IotSupplyStore.Models.UpsertModel;
using IotSupplyStore.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using System.Net;

namespace IotSupplyStore.Controllers
{
    [ApiController]
    //[Authorize]
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
        public async Task<IActionResult> GetItemsFromCart()
        {
            var UserId = User.Claims.FirstOrDefault(u => u.Type == "id").Value;
            var CartsList = await _db.ShoppingCarts.Where(u => u.ApplicationUserId == UserId).ToListAsync();

            return Ok(CartsList);
        }

        [HttpPost]
        public async Task<IActionResult> MakeAnOrder(List<ShoppingCartUpsert> items)
        {
            var CustomerId = User.Claims.FirstOrDefault(u => u.Type == "id").Value;
            var Customer = await _db.User.FirstOrDefaultAsync(User => User.Id == CustomerId);
            float OrderTotal = 0;

            var ProductOrdersList = new List<ProductOrder>();
            var product = await _db.Products.ToListAsync();

            _db.Orders.Add(new Order()
            {
                CustomerName = Customer.FullName,
                PhoneNumber = Customer.PhoneNumber,
                Address = Customer.Address,
                ApplicationUserId = CustomerId
            });
            _db.SaveChanges();
            var OrderId = _db.Orders.ToList().LastOrDefault(u => u.ApplicationUserId == CustomerId).Id;

            foreach (var item in items)
            {
                var CheckQuantityFromRepository = _db.Products.FirstOrDefault(u => u.Id == item.ProductId).Quantity;
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
                    OrderId = OrderId,
                    Price = product.FirstOrDefault(u => u.Id == item.ProductId).Price * item.Count
                });
                OrderTotal += ProductOrdersList.Last().Price;

                product.FirstOrDefault(u => u.Id == item.ProductId).Quantity = CheckQuantityFromRepository - item.Count;
            }

            _db.ProductOrders.AddRange(ProductOrdersList);

            var order = _db.Orders.ToList().LastOrDefault(u => u.Id == OrderId);
            order.OrderTotal = OrderTotal;

            _db.Orders.Update(order);
            await _db.SaveChangesAsync();

            _response.StatusCode = HttpStatusCode.OK;
            _response.Message = "Added successfully";
            _response.Result = ProductOrdersList;
            _response.ErrorMessages = null;
            return new JsonResult(_response);
        }
    }
}
