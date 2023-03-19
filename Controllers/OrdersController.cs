using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IotSupplyStore.DataAccess;
using IotSupplyStore.Models;

namespace IotSupplyStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        // Lấy danh sách tất cả các đối tượng Order trong cơ sở dữ liệu
        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _context.Orders.ToListAsync();
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Orders.AsNoTracking().Include(x => x.ProductOrders).ThenInclude(x => x.Product).SingleOrDefaultAsync(x => x.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order orderDto)
        {
            if (id != orderDto.Id)
            {
                return BadRequest();
            }
            var order = await _context.Orders.SingleOrDefaultAsync(x => x.Id == orderDto.Id);
            order.Or_Quantity = orderDto.Or_Quantity;
            order.ProductOrders = orderDto.ProductOrders.Select(x => new ProductOrder
            {
                ProductId = x.ProductId
            }).ToList();
            //_context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            var userid = User.Claims.Where(x => x.Type == "id").FirstOrDefault()?.Value;
            var orderCreate = new Order
            {
                Transactions = order.Transactions,
                Or_Price = order.Or_Price,
                Or_PriceSale = order.Or_PriceSale,
                Or_Quantity = order.Or_Quantity,
                ApplicationUserId = userid,
                /*Id = order.Id,
                ProductOrders = order.ProductOrders.Select(x => new ProductOrder
                {
                    ProductId = x.ProductId,
                    OrderId = x=orderCreate.Id
                }).ToList()*/
            };
            _context.Orders.Add(orderCreate);
            await _context.SaveChangesAsync();

            // Gán giá trị của thuộc tính Id của đối tượng Order cho thuộc tính OrderId của các đối tượng ProductOrder
            //order.ProductOrders.ForEach(x => x.OrderId = orderCreate.Id);
            foreach (var item in order.ProductOrders)
            {
                item.OrderId = orderCreate.Id;
            }
            _context.ProductOrders.AddRange(order.ProductOrders);
            await _context.SaveChangesAsync();

            //order.ProductOrders.ForEach(x => x.Order = null);
            foreach (var item in order.ProductOrders)
            {
                item.Order = null;
            }
            return order;
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
