using Azure;
using IotSupplyStore.DataAccess;
using IotSupplyStore.Models;
using IotSupplyStore.Models.DtoModel;
using IotSupplyStore.Models.ViewModel;
using IotSupplyStore.Repository.IRepository;
using IotSupplyStore.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace IotSupplyStore.Controllers.Employee
{
    [Authorize(Policy = SD.Role_Shipper)]
    [ApiController]
    [Route("api/shipper")]
    public class ShipperController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private ApiResponse _response;
        public ShipperController(IUnitOfWork db, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = db;
            _userManager = userManager;
            _response = new ApiResponse();
        }

        [HttpGet("get-list-order-pending")]
        public async Task<IActionResult> GetListOrderIsPending()
        {
            var ListOrders = await _unitOfWork.Order.GetAllAsync(x => x.OrderStatus == false);
            if (ListOrders == null)
            {
                _response.Message = "Not Found";
                _response.Result = null;
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("Cannot find any order pending at the moment");
                return new JsonResult(_response);
            }

            _response.Message = "Take Orders List Successfully";
            _response.Result = ListOrders;
            _response.StatusCode = HttpStatusCode.OK;
            _response.ErrorMessages = null;
            return new JsonResult(_response);
        }

        [HttpPost("take-order/{orderId}")]
        public async Task<IActionResult> TakeOrder(string orderId)
        {
            var OrderPending = await _unitOfWork.Order.GetFirstOrDefaultAsync(u => u.Id ==  orderId && u.OrderStatus == false);
            
            if (OrderPending == null)
            {
                _response.Message = "Not Found";
                _response.Result = null;
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("This order has been taken from another shipper");
                return new JsonResult(_response);
            }
            OrderPending.OrderStatus = true;

            var ShipperId = User.Claims.FirstOrDefault(u => u.Type == "id").Value;

            var orderStatus = new OrderStatus()
            {
                OrderId = orderId,
                ShipperId = ShipperId,
                StatusInProcess = true
            };

            await _unitOfWork.OrderStatus.Add(orderStatus);
            await _unitOfWork.Save();

            _response.Message = "Take Order Successfully";
            _response.Result = orderStatus;
            _response.StatusCode = HttpStatusCode.OK;
            _response.ErrorMessages = null;
            return new JsonResult(_response);
        }

        [HttpPost("mark-as-shipped/{orderId}")]
        public async Task<IActionResult> CompleteShipping(string orderId)
        {
            var orderStatus = await _unitOfWork.OrderStatus.GetFirstOrDefaultAsync(u => u.OrderId == orderId);

            if (orderStatus == null)
            {
                _response.Message = "Not Found";
                _response.Result = null;
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("This order shipped");
                return new JsonResult(_response);
            }

            orderStatus.StatusShipped = true;
            orderStatus.ShippingDate = DateTime.Now;
            _unitOfWork.OrderStatus.Update(orderStatus);
            await _unitOfWork.Save();

            _response.Message = "Mark Complete Successfully";
            _response.Result = orderStatus;
            _response.StatusCode = HttpStatusCode.OK;
            _response.ErrorMessages = null;
            return new JsonResult(_response);
        }

        [HttpPost("cancel-order/{orderId}")]
        public async Task<IActionResult> CancelOrder(string orderId)
        {
            var orderStatus = await _unitOfWork.OrderStatus.GetFirstOrDefaultAsync(u => u.OrderId == orderId);

            if (orderStatus == null)
            {
                _response.Message = "Not Found";
                _response.Result = null;
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add($"Cannot find this order with id {orderId}");
                return new JsonResult(_response);
            }

            orderStatus.StatusCancelled = true;
            _unitOfWork.OrderStatus.Update(orderStatus);
            await _unitOfWork.Save();

            _response.Message = "Cancel Successfully";
            _response.Result = orderStatus;
            _response.StatusCode = HttpStatusCode.OK;
            _response.ErrorMessages = null;
            return new JsonResult(_response);
        }

        [HttpPost("accepted-payment/{orderId}")]
        public async Task<IActionResult> AcceptPayment(string orderId)
        {
            var orderStatus = await _unitOfWork.OrderStatus.GetFirstOrDefaultAsync(u => u.OrderId == orderId);
            var order = await _unitOfWork.Order.GetFirstOrDefaultAsync(u => u.Id == orderId);

            if (orderStatus == null)
            {
                _response.Message = "Not Found";
                _response.Result = null;
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add($"Cannot find this order with id {orderId}");
                return new JsonResult(_response);
            }

            order.PaymentStatus = true;
            orderStatus.StatusPayment = true;
            orderStatus.PaymentDate = DateTime.Now;

            _unitOfWork.Order.Update(order);
            _unitOfWork.OrderStatus.Update(orderStatus);
            await _unitOfWork.Save();

            _response.Message = "Mark as Paid Successfully";
            _response.Result = orderStatus;
            _response.StatusCode = HttpStatusCode.OK;
            _response.ErrorMessages = null;
            return new JsonResult(_response);
        }

        #region admin area

        [HttpGet]
        [Authorize(Roles = SD.Role_Admin)]
        public async Task<IActionResult> GetAllShipper()
        {
            var ShipperList = await _userManager.GetUsersInRoleAsync(SD.Role_Shipper);
            if (ShipperList == null)
            {
                return NotFound("No shipper found");
            }
            return Ok(ShipperList);
        }
        [Authorize(Roles = SD.Role_Admin)]
        [HttpGet("id")]
        public async Task<IActionResult> GetShipperById(string id)
        {
            var Shipper = await _unitOfWork.ApplicationUser.GetFirstOrDefaultAsync(u => u.Id == id, false);
            if (Shipper == null)
            {
                return NotFound($"this shipper with id {id} not found");
            }
            return Ok(Shipper);
        }

        #endregion

    }
}
