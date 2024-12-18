using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServerPhB.Models;
using ServerPhB.Services;

namespace ServerPhB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ManagerController : ControllerBase
    {
        private readonly ServerPhB.Services.OrderService _orderService;

        public ManagerController(ServerPhB.Services.OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("assign-order")]
        public IActionResult AssignOrderToSalon([FromBody] Order assignment)
        {
            // TODO: Implement this method
            return Ok();
        }

        [HttpGet("track-order/{orderId}")]
        [Authorize]
        public IActionResult TrackOrder(int orderId)
        {
            // TODO: Implement this method
            return Ok();
        }

        [HttpGet("view-orders")]
        [Authorize]
        public IActionResult ViewOrderList()
        {
            // TODO: Implement this method
            return Ok();
        }
    }
}
