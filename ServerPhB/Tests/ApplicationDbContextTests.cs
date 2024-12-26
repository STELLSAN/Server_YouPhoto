using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using ServerPhB.Data;
using ServerPhB.Models;
using System;
using System.Linq;
using System.Collections.Generic;

namespace ServerPhB.Tests
{
    public class ApplicationDbContextTests
    {
        private DbContextOptions<ApplicationDbContext> _options;

        [SetUp]
        public void Setup()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
        }

        [Test]
        public void ApplicationDbContext_CanBeInitialized()
        {
            using (var context = new ApplicationDbContext(_options))
            {
                Assert.DoesNotThrow(() => new ApplicationDbContext(_options));
            }
        }

        [Test]
        public void ApplicationDbContext_HasInitialDeliveryMethods()
        {
            using (var context = new ApplicationDbContext(_options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                var deliveryMethods = context.DeliveryMethods.ToList();

                Assert.AreEqual(2, deliveryMethods.Count);
                Assert.IsTrue(deliveryMethods.Any(dm => dm.DeliveryMethodID == 1 && dm.Name == "Доставка" && dm.Cost == 500));
                Assert.IsTrue(deliveryMethods.Any(dm => dm.DeliveryMethodID == 2 && dm.Name == "Само-вывоз" && dm.Cost == 0));
            }
        }

        [Test]
        public void ApplicationDbContext_CanAddAndRetrieveUsers()
        {
            using (var context = new ApplicationDbContext(_options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                var user = new User
                {
                    UserID = 1,
                    Name = "Test User",
                    Email = "test@example.com",
                    PasswordHash = "hashed_password",
                    Phone = "1234567890",
                    Username = "testuser"
                };
                context.Users.Add(user);
                context.SaveChanges();

                var retrievedUser = context.Users.FirstOrDefault(u => u.UserID == 1);

                Assert.IsNotNull(retrievedUser);
                Assert.AreEqual(user.Name, retrievedUser.Name);
                Assert.AreEqual(user.Email, retrievedUser.Email);
                Assert.AreEqual(user.PasswordHash, retrievedUser.PasswordHash);
                Assert.AreEqual(user.Phone, retrievedUser.Phone);
                Assert.AreEqual(user.Username, retrievedUser.Username);
            }
        }

        [Test]
        public void ApplicationDbContext_CanAddAndRetrieveOrders()
        {
            using (var context = new ApplicationDbContext(_options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                var order = new Order
                {
                    OrderID = 1,
                    DateCreated = DateTime.Now,
                    Status = "Pending",
                    ClientID = "12345",
                    DeliveryMethodID = "1",
                    Address = "123 Main St",
                    DecorationOptionID = 1,
                    Comments = "Urgent order",
                    TotalPrice = 1000,
                    Quantity = 2,
                    Format = "4x6",
                    PhotoPaths = new List<string> { "path1.jpg", "path2.jpg" }
                };
                context.Orders.Add(order);
                context.SaveChanges();

                var retrievedOrder = context.Orders.FirstOrDefault(o => o.OrderID == 1);

                Assert.IsNotNull(retrievedOrder);
                Assert.AreEqual(order.Status, retrievedOrder.Status);
                Assert.AreEqual(order.ClientID, retrievedOrder.ClientID);
                Assert.AreEqual(order.Address, retrievedOrder.Address);
                Assert.AreEqual(order.TotalPrice, retrievedOrder.TotalPrice);
                Assert.AreEqual(order.Quantity, retrievedOrder.Quantity);
                Assert.AreEqual(order.Format, retrievedOrder.Format);
                Assert.AreEqual(order.PhotoPaths.Count, retrievedOrder.PhotoPaths.Count);
            }
        }
    }
}
