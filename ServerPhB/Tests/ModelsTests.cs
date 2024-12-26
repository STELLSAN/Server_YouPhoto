using NUnit.Framework;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using ServerPhB.Models;

namespace ServerPhB.Tests
{
    [TestFixture]
    public class ModelsTests
    {
        [Test]
        public void CreateOrderRequest_TestGettersAndSetters()
        {
            var photos = new List<IFormFile>();
            var request = new CreateOrderRequest
            {
                Photos = photos,
                Format = "A4",
                Quantity = 10,
                DecorationOptionID = 1,
                DeliveryMethodID = 2,
                Address = "Test Address",
                Comments = "Test Comments",
                TotalPrice = 100
            };

            Assert.AreEqual(photos, request.Photos);
            Assert.AreEqual("A4", request.Format);
            Assert.AreEqual(10, request.Quantity);
            Assert.AreEqual(1, request.DecorationOptionID);
            Assert.AreEqual(2, request.DeliveryMethodID);
            Assert.AreEqual("Test Address", request.Address);
            Assert.AreEqual("Test Comments", request.Comments);
            Assert.AreEqual(100, request.TotalPrice);
        }

        [Test]
        public void DeliveryMethod_TestGettersAndSetters()
        {
            var method = new DeliveryMethod
            {
                DeliveryMethodID = 1,
                Name = "Courier",
                Description = "Fast delivery",
                Cost = 50
            };

            Assert.AreEqual(1, method.DeliveryMethodID);
            Assert.AreEqual("Courier", method.Name);
            Assert.AreEqual("Fast delivery", method.Description);
            Assert.AreEqual(50, method.Cost);
        }

        [Test]
        public void Equipment_TestGettersAndSetters()
        {
            var equipment = new Equipment
            {
                EquipmentID = 1,
                Name = "Printer",
                Description = "High-quality printer",
                Cost = 1000
            };

            Assert.AreEqual(1, equipment.EquipmentID);
            Assert.AreEqual("Printer", equipment.Name);
            Assert.AreEqual("High-quality printer", equipment.Description);
            Assert.AreEqual(1000, equipment.Cost);
        }

        [Test]
        public void LoginRequest_TestGettersAndSetters()
        {
            var loginRequest = new LoginRequest
            {
                Username = "testuser",
                Password = "password123"
            };

            Assert.AreEqual("testuser", loginRequest.Username);
            Assert.AreEqual("password123", loginRequest.Password);
        }

        [Test]
        public void Order_TestGettersAndSetters()
        {
            var photoPaths = new List<string> { "photo1.jpg", "photo2.jpg" };
            var order = new Order
            {
                OrderID = 1,
                DateCreated = DateTime.Now,
                Status = "Pending",
                ClientID = "Client1",
                DeliveryMethodID = "1",
                Address = "Test Address",
                DecorationOptionID = 1,
                Comments = "Test Comments",
                TotalPrice = 200,
                Quantity = 5,
                Format = "A4",
                PhotoPaths = photoPaths
            };

            Assert.AreEqual(1, order.OrderID);
            Assert.AreEqual("Pending", order.Status);
            Assert.AreEqual("Client1", order.ClientID);
            Assert.AreEqual("1", order.DeliveryMethodID);
            Assert.AreEqual("Test Address", order.Address);
            Assert.AreEqual(1, order.DecorationOptionID);
            Assert.AreEqual("Test Comments", order.Comments);
            Assert.AreEqual(200, order.TotalPrice);
            Assert.AreEqual(5, order.Quantity);
            Assert.AreEqual("A4", order.Format);
            Assert.AreEqual(photoPaths, order.PhotoPaths);
        }

        [Test]
        public void OrderDto_TestGettersAndSetters()
        {
            var orderDto = new OrderDto
            {
                OrderID = 1,
                DateCreated = DateTime.Now,
                Status = "Completed",
                ClientID = "Client1",
                DeliveryMethodID = "1",
                Address = "Test Address",
                DecorationOptionID = 1,
                Comments = "Delivered",
                TotalPrice = 300,
                Quantity = 10
            };

            Assert.AreEqual(1, orderDto.OrderID);
            Assert.AreEqual("Completed", orderDto.Status);
            Assert.AreEqual("Client1", orderDto.ClientID);
            Assert.AreEqual("1", orderDto.DeliveryMethodID);
            Assert.AreEqual("Test Address", orderDto.Address);
            Assert.AreEqual(1, orderDto.DecorationOptionID);
            Assert.AreEqual("Delivered", orderDto.Comments);
            Assert.AreEqual(300, orderDto.TotalPrice);
            Assert.AreEqual(10, orderDto.Quantity);
        }

        [Test]
        public void Photo_TestGettersAndSetters()
        {
            var photo = new Photo
            {
                PhotoID = 1,
                FileName = "image.jpg",
                FilePath = "C:/images/image.jpg",
                FileSize = 1024
            };

            Assert.AreEqual(1, photo.PhotoID);
            Assert.AreEqual("image.jpg", photo.FileName);
            Assert.AreEqual("C:/images/image.jpg", photo.FilePath);
            Assert.AreEqual(1024, photo.FileSize);
        }

        [Test]
        public void RefreshTokenRequest_TestGettersAndSetters()
        {
            var refreshTokenRequest = new RefreshTokenRequest
            {
                Token = "refresh-token"
            };

            Assert.AreEqual("refresh-token", refreshTokenRequest.Token);
        }

        [Test]
        public void RegisterRequest_TestGettersAndSetters()
        {
            var registerRequest = new RegisterRequest
            {
                Username = "testuser",
                Password = "password123",
                Name = "Test User",
                Email = "test@example.com",
                Phone = "1234567890",
                Role = 1
            };

            Assert.AreEqual("testuser", registerRequest.Username);
            Assert.AreEqual("password123", registerRequest.Password);
            Assert.AreEqual("Test User", registerRequest.Name);
            Assert.AreEqual("test@example.com", registerRequest.Email);
            Assert.AreEqual("1234567890", registerRequest.Phone);
            Assert.AreEqual(1, registerRequest.Role);
        }

        [Test]
        public void ResponseOrderWithPhotos_TestGettersAndSetters()
        {
            var photos = new List<string> { "photo1", "photo2" };
            var responseOrderWithPhotos = new ResponseOrderWithPhotos
            {
                OrderID = 1,
                DateCreated = DateTime.Now,
                Status = "Delivered",
                ClientID = "Client1",
                DeliveryMethodID = "1",
                Address = "Test Address",
                DecorationOptionID = 1,
                Comments = "All good",
                TotalPrice = 400,
                Quantity = 20,
                Photos = photos
            };

            Assert.AreEqual(1, responseOrderWithPhotos.OrderID);
            Assert.AreEqual("Delivered", responseOrderWithPhotos.Status);
            Assert.AreEqual("Client1", responseOrderWithPhotos.ClientID);
            Assert.AreEqual("1", responseOrderWithPhotos.DeliveryMethodID);
            Assert.AreEqual("Test Address", responseOrderWithPhotos.Address);
            Assert.AreEqual(1, responseOrderWithPhotos.DecorationOptionID);
            Assert.AreEqual("All good", responseOrderWithPhotos.Comments);
            Assert.AreEqual(400, responseOrderWithPhotos.TotalPrice);
            Assert.AreEqual(20, responseOrderWithPhotos.Quantity);
            Assert.AreEqual(photos, responseOrderWithPhotos.Photos);
        }

        [Test]
        public void UpdateOrderRequest_TestGettersAndSetters()
        {
            var photos = new List<IFormFile>();
            var updateOrderRequest = new UpdateOrderRequest
            {
                OrderID = 1,
                Status = "In Progress",
                Address = "New Address",
                DecorationOptionID = 2,
                Comments = "Updated Comments",
                TotalPrice = 500,
                Quantity = 15,
                Photos = photos
            };

            Assert.AreEqual(1, updateOrderRequest.OrderID);
            Assert.AreEqual("In Progress", updateOrderRequest.Status);
            Assert.AreEqual("New Address", updateOrderRequest.Address);
            Assert.AreEqual(2, updateOrderRequest.DecorationOptionID);
            Assert.AreEqual("Updated Comments", updateOrderRequest.Comments);
            Assert.AreEqual(500, updateOrderRequest.TotalPrice);
            Assert.AreEqual(15, updateOrderRequest.Quantity);
            Assert.AreEqual(photos, updateOrderRequest.Photos);
        }

        [Test]
        public void User_TestGettersAndSetters()
        {
            var user = new User
            {
                UserID = 1,
                Username = "testuser",
                Role = 1,
                PasswordHash = "hashedpassword",
                Name = "Test User",
                Email = "test@example.com",
                Phone = "1234567890"
            };

            Assert.AreEqual(1, user.UserID);
            Assert.AreEqual("testuser", user.Username);
            Assert.AreEqual(1, user.Role);
            Assert.AreEqual("hashedpassword", user.PasswordHash);
            Assert.AreEqual("Test User", user.Name);
            Assert.AreEqual("test@example.com", user.Email);
            Assert.AreEqual("1234567890", user.Phone);
        }

        [Test]
        public void Authentication_CanSetAndGetProperties()
        {
            var auth = new Authentication
            {
                Token = "test-token",
                ExpirationDate = new DateTime(2024, 12, 31)
            };

            Assert.AreEqual("test-token", auth.Token);
            Assert.AreEqual(new DateTime(2024, 12, 31), auth.ExpirationDate);
        }

        [Test]
        public void Client_CanSetAndGetProperties()
        {
            var client = new Client
            {
                Name = "John Doe",
                Email = "john.doe@example.com",
                Phone = "123-456-7890"
            };

            Assert.AreEqual("John Doe", client.Name);
            Assert.AreEqual("john.doe@example.com", client.Email);
            Assert.AreEqual("123-456-7890", client.Phone);
        }

        [Test]
        public void DatabaseHandler_PrivateConnectionStringCannotBeTested()
        {
            Assert.Pass("No public getter or setter available for ConnectionString.");
        }

        [Test]
        public void DecorationOption_CanSetAndGetProperties()
        {
            var option = new DecorationOption
            {
                DecorationOptionID = 1,
                Name = "Balloons",
                Description = "Colorful balloons for decoration.",
                Cost = 50
            };

            Assert.AreEqual(1, option.DecorationOptionID);
            Assert.AreEqual("Balloons", option.Name);
            Assert.AreEqual("Colorful balloons for decoration.", option.Description);
            Assert.AreEqual(50, option.Cost);
        }

        [Test]
        public void Manager_CanSetAndGetProperties()
        {
            var manager = new Manager
            {
                Name = "Jane Smith",
                Email = "jane.smith@example.com",
                Phone = "987-654-3210"
            };

            Assert.AreEqual("Jane Smith", manager.Name);
            Assert.AreEqual("jane.smith@example.com", manager.Email);
            Assert.AreEqual("987-654-3210", manager.Phone);
        }

        [Test]
        public void OrderItem_CanSetAndGetProperties()
        {
            var orderItem = new OrderItem
            {
                OrderItemID = 10,
                OrderID = 20,
                DecorationOptions = 5,
                Quantity = 100
            };

            Assert.AreEqual(10, orderItem.OrderItemID);
            Assert.AreEqual(20, orderItem.OrderID);
            Assert.AreEqual(5, orderItem.DecorationOptions);
            Assert.AreEqual(100, orderItem.Quantity);
        }

        [Test]
        public void OrderService_CanSetAndGetProperties()
        {
            var service = new OrderService
            {
                OrderServiceID = 42
            };

            Assert.AreEqual(42, service.OrderServiceID);
        }

        [Test]
        public void OrderUpdate_CanSetAndGetProperties()
        {
            var update = new OrderUpdate
            {
                UpdateID = 101,
                OrderID = 202,
                Status = "In Progress"
            };

            Assert.AreEqual(101, update.UpdateID);
            Assert.AreEqual(202, update.OrderID);
            Assert.AreEqual("In Progress", update.Status);
        }

        [Test]
        public void Salon_CanSetAndGetProperties()
        {
            var salon = new Salon
            {
                SalonID = 1,
                Name = "Luxury Salon",
                Address = "123 Main Street",
                EquipmentList = new List<Equipment> { new Equipment(), new Equipment() }
            };

            Assert.AreEqual(1, salon.SalonID);
            Assert.AreEqual("Luxury Salon", salon.Name);
            Assert.AreEqual("123 Main Street", salon.Address);
            Assert.AreEqual(2, salon.EquipmentList.Count);
        }


    }
}
