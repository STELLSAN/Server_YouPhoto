using System;
using Microsoft.AspNetCore.Mvc;
using ServerPhB.Models;
using ServerPhB.Services;

namespace ServerPhB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly ServerPhB.Services.OrderService _orderService;

        public ClientController(ServerPhB.Services.OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("upload-photo")]
        public IActionResult UploadPhoto([FromBody] Photo photo)
        {
            // TODO: Implement this method
            return Ok();
        }

        [HttpPost("create-order")]
        public IActionResult CreateOrder([FromBody] Order order)
        {
            // TODO: Implement this method
            return Ok();
        }

        [HttpGet("track-order/{orderId}")]
        public IActionResult TrackOrder(int orderId)
        {
            // TODO: Implement this method
            return Ok();
        }
    }
}
