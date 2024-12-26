using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using ServerPhB.Controllers;
using ServerPhB.Data;
using ServerPhB.Models;

namespace ServerPhB.Tests
{
    [TestFixture]
    public class DeliveryMethodsControllerTests
    {
        private ApplicationDbContext _context;
        private DeliveryMethodController _controller;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);
            _controller = new DeliveryMethodController(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task GetDeliveryMethods_ReturnsOkResult_WithDeliveryMethods()
        {
            // Arrange
            var deliveryMethods = new List<DeliveryMethod>
            {
                new DeliveryMethod { DeliveryMethodID = 1, Name = "Доставка", Cost = 500 },
                new DeliveryMethod { DeliveryMethodID = 2, Name = "Само-вывоз", Cost = 0 }
            };

            await _context.DeliveryMethods.AddRangeAsync(deliveryMethods);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetDeliveryMethods();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            var deliveryMethodList = okResult.Value as List<DeliveryMethod>;
            Assert.AreEqual(2, deliveryMethodList.Count);
            Assert.AreEqual(1, deliveryMethodList[0].DeliveryMethodID);
            Assert.AreEqual(2, deliveryMethodList[1].DeliveryMethodID);
        }

        [Test]
        public async Task GetDeliveryMethods_ReturnsOkResult_WithNoDeliveryMethods()
        {
            // Act
            var result = await _controller.GetDeliveryMethods();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            var deliveryMethodList = okResult.Value as List<DeliveryMethod>;
            Assert.AreEqual(0, deliveryMethodList.Count);
        }
    }
}
