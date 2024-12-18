using System;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public IActionResult UploadPhoto([FromBody] Photo photo)
        {
            // TODO: Implement this method
            return Ok();
        }

        [HttpPost("create-order")]
        [Authorize]
        public IActionResult CreateOrder([FromBody] Order order)
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
    }
}
