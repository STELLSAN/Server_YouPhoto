using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerPhB.Models;
using ServerPhB.Services;
using OrderService = ServerPhB.Services.OrderService;

namespace ServerPhB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;
        private readonly IWebHostEnvironment _environment;

        public OrderController(OrderService orderService, IWebHostEnvironment environment)
        {
            _orderService = orderService;
            _environment = environment;
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreateOrder([FromForm] CreateOrderRequest request)
        {
            var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (clientId == null)
            {
                return Unauthorized(new { message = "Invalid token" });
            }

            if (request.DeliveryMethodID == 1 && string.IsNullOrEmpty(request.Address))
            {
                return BadRequest(new { message = "Address is required for home delivery" });
            }

            var photoPaths = new List<string>();
            if (request.Photos != null && request.Photos.Count > 0)
            {
                foreach (var photo in request.Photos)
                {
                    var filePath = Path.Combine(_environment.ContentRootPath, "Assets/User", photo.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await photo.CopyToAsync(stream);
                    }
                    photoPaths.Add(filePath);
                }
            }

            var order = new Order
            {
                DateCreated = DateTime.UtcNow,
                Status = "Pending",
                ClientID = clientId,
                DeliveryMethodID = request.DeliveryMethodID.ToString(),
                Address = string.IsNullOrEmpty(request.Address) ? null : request.Address,
                DecorationOptionID = request.DecorationOptionID,
                Comments = string.IsNullOrEmpty(request.Comments) ? null : request.Comments,
                TotalPrice = request.TotalPrice,
                Quantity = request.Quantity,
                Format = request.Format,
                PhotoPaths = photoPaths
            };

            _orderService.CreateOrder(order);
            return Ok(new { message = "Order created successfully" });
        }

        [HttpPost("update")]
        [Authorize]
        public async Task<IActionResult> UpdateOrder([FromForm] UpdateOrderRequest request)
        {
            var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (clientId == null)
            {
                return Unauthorized(new { message = "Invalid token" });
            }

            var order = _orderService.GetOrderById(request.OrderID);
            if (order == null || order.ClientID != clientId)
            {
                return NotFound(new { message = "Order not found or you do not have permission to update this order" });
            }

            // Update the fields
            if (!string.IsNullOrEmpty(request.Status)) order.Status = request.Status;
            if (!string.IsNullOrEmpty(request.Address)) order.Address = request.Address;
            if (request.DecorationOptionID.HasValue) order.DecorationOptionID = request.DecorationOptionID.Value;
            if (!string.IsNullOrEmpty(request.Comments)) order.Comments = request.Comments;
            if (request.TotalPrice.HasValue) order.TotalPrice = request.TotalPrice.Value;
            if (request.Quantity.HasValue) order.Quantity = request.Quantity.Value;

            // Handle new photo uploads
            if (request.Photos != null && request.Photos.Count > 0)
            {
                var photoPaths = new List<string>();
                foreach (var photo in request.Photos)
                {
                    var filePath = Path.Combine(_environment.WebRootPath, "Assets/User", photo.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await photo.CopyToAsync(stream);
                    }
                    photoPaths.Add(filePath);
                }
                order.PhotoPaths.AddRange(photoPaths); // Add new photo paths to existing ones
            }

            _orderService.UpdateOrder(order);
            return Ok(new { message = "Order updated successfully" });
        }

        [HttpGet("track/{orderId}")]
        public IActionResult TrackOrder(int orderId)
        {
            var order = _orderService.GetOrderById(orderId);
            if (order == null)
            {
                return NotFound(new { message = "Order not found" });
            }

            var orderWithPhotos = new ResponseOrderWithPhotos
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
                Quantity = order.Quantity,
                Photos = new List<string>()
            };

            foreach (var photoPath in order.PhotoPaths)
            {
                var photoBytes = System.IO.File.ReadAllBytes(photoPath);
                var photoBase64 = Convert.ToBase64String(photoBytes);
                orderWithPhotos.Photos.Add(photoBase64);
            }

            return Ok(orderWithPhotos);
        }

        [HttpGet("fetch-user-orders")]
        [Authorize]
        public async Task<IActionResult> GetUserOrders()
        {
            var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (clientId == null)
            {
                return Unauthorized(new { message = "Invalid token" });
            }

            var orders = await _orderService.GetOrdersByClientId(clientId);
            var orderDtos = orders.Select(order => new OrderDto
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
