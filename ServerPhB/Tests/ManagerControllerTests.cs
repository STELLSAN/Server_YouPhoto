using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using ServerPhB.Controllers;
using ServerPhB.Data;
using ServerPhB.Models;
using ServerPhB.Services;

namespace ServerPhB.Tests
{
    [TestFixture]
    public class ManagerControllerTests
    {
        private ApplicationDbContext _context;
        private OrderService _orderService;
        private ManagerController _controller;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);
            _orderService = new OrderService(_context);
            _controller = new ManagerController(_orderService);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "testManagerId"),
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
        public async Task ViewOrderList_ReturnsOkResult_WithPendingOrders()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order { OrderID = 1, Status = "Pending", ClientID = "testClientId", DeliveryMethodID = "1", Format = "A4", PhotoPaths = new List<string>() },
                new Order { OrderID = 2, Status = "In Progress", ClientID = "testClientId", DeliveryMethodID = "1", Format = "A4", PhotoPaths = new List<string>() }
            };

            await _context.Orders.AddRangeAsync(orders);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.ViewOrderList();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            var orderDtos = okResult.Value as List<OrderDto>;
            Assert.AreEqual(2, orderDtos.Count);
            Assert.AreEqual(1, orderDtos[0].OrderID);
            Assert.AreEqual(2, orderDtos[1].OrderID);
        }

        [Test]
        public async Task ViewOrderList_ReturnsOkResult_WithNoOrders()
        {
            // Act
            var result = await _controller.ViewOrderList();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            var orderDtos = okResult.Value as List<OrderDto>;
            Assert.AreEqual(0, orderDtos.Count);
        }
    }
}
