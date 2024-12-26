using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using ServerPhB.Controllers;
using ServerPhB.Data;
using ServerPhB.Models;
using ServerPhB.Services;

namespace ServerPhB.Tests
{
    [TestFixture]
    public class OrderControllerTests
    {
        private ApplicationDbContext _context;
        private OrderService _orderService;
        private Mock<IWebHostEnvironment> _environmentMock;
        private OrderController _controller;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);
            _orderService = new OrderService(_context);
            _environmentMock = new Mock<IWebHostEnvironment>();
            _controller = new OrderController(_orderService, _environmentMock.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "testClientId"),
                new Claim(ClaimTypes.Role, "1")
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task CreateOrder_ReturnsOkResult_WhenOrderIsCreated()
        {
            // Arrange
            var request = new CreateOrderRequest
            {
                Photos = new List<IFormFile>(),
                Format = "A4",
                Quantity = 10,
                DecorationOptionID = 1,
                DeliveryMethodID = 2,
                Address = "Test Address",
                Comments = "Test Comments",
                TotalPrice = 100
            };

            // Act
            var result = await _controller.CreateOrder(request);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual("Order created successfully", okResult.Value.GetType().GetProperty("message").GetValue(okResult.Value, null));
        }

        [Test]
        public async Task UpdateOrder_ReturnsOkResult_WhenOrderIsUpdated()
        {
            // Arrange
            var order = new Order
            {
                OrderID = 1,
                ClientID = "testClientId",
                Status = "Pending",
                Address = "Old Address",
                DecorationOptionID = 1,
                Comments = "Old Comments",
                TotalPrice = 100,
                Quantity = 10,
                Format = "A4",
                PhotoPaths = new List<string>(),
                DeliveryMethodID = "2"
            };

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            var request = new UpdateOrderRequest
            {
                OrderID = 1,
                Status = "In Progress",
                Address = "New Address",
                DecorationOptionID = 2,
                Comments = "Updated Comments",
                TotalPrice = 500,
                Quantity = 15,
                Photos = new List<IFormFile>()
            };

            // Act
            var result = await _controller.UpdateOrder(request);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual("Order updated successfully", okResult.Value.GetType().GetProperty("message").GetValue(okResult.Value, null));
        }

        [Test]
        public async Task GetUserOrders_ReturnsUnauthorized_WhenClientIdIsNull()
        {
            // Arrange
            _controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());

            // Act
            var result = await _controller.GetUserOrders();

            // Assert
            Assert.IsInstanceOf<UnauthorizedObjectResult>(result);
            var unauthorizedResult = result as UnauthorizedObjectResult;
            Assert.AreEqual("Invalid token", unauthorizedResult.Value.GetType().GetProperty("message").GetValue(unauthorizedResult.Value, null));
        }
    }
}
