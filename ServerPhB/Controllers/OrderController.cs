using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServerPhB.Models;
using ServerPhB.Services;

namespace ServerPhB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ServerPhB.Services.OrderService _orderService;

        public OrderController(ServerPhB.Services.OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("create")]
        public IActionResult CreateOrder([FromBody] Order order)
        {
            _orderService.CreateOrder(order);
            return Ok(new { message = "Order created successfully" });
        }

        [HttpPost("update")]
        public IActionResult UpdateOrder([FromBody] Order order)
        {
            _orderService.UpdateOrder(order);
            return Ok(new { message = "Order updated successfully" });
        }

        [HttpGet("track/{orderId}")]
        [Authorize]
        public IActionResult TrackOrder(int orderId)
        {
            var order = _orderService.TrackOrder(orderId);
            return Ok(order);
        }
    }
}
