using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderService.Data;
using OrderService.Models;
using OrderService.Services;
using System.Text.Json;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly RabbitMQService _rabbitMQ;
        private readonly AppDbContext _context;

        public OrdersController(RabbitMQService rabbitMQ, AppDbContext context)
        {
            _rabbitMQ = rabbitMQ;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder()
        {
            // Simulate order creation
            var order = new Order
            {
                OrderId = Guid.NewGuid(),
                Amount = 100
            };

            // Create order in DB
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Send message to RabbitMQ
            var json = JsonSerializer.Serialize(order);
            _rabbitMQ.SendMessage(json);

            return Ok(new { Message = "Order Created", OrderId = order.OrderId, Amount = order.Amount });
        }
    }
}
