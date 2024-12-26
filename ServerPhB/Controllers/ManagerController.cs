using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerPhB.Models;
using ServerPhB.Services;
using OrderService = ServerPhB.Services.OrderService;

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

        [HttpGet("view-orders")]
        [Authorize]
        public async Task<IActionResult> ViewOrderList()
        {
            var pendingOrders = await _orderService.GetOrdersByStatusExcluding("Done");

            var orderDtos = pendingOrders.Select(order => new OrderDto
            {
                OrderID = order.OrderID,
                DateCreated = order.DateCreated,
                Status = order.Status,
                ClientID = order.ClientID,
                DeliveryMethodID = order.DeliveryMethodID,
                Address = order.Address,
                DecorationOptionID = order.DecorationOptionID,
                Comments = order.Comments,
                TotalPrice = order.TotalPrice,
                Quantity = order.Quantity
            }).ToList();

            return Ok(orderDtos);
        }
    }
}
