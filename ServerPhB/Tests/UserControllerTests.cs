using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using ServerPhB.Controllers;
using ServerPhB.Data;
using ServerPhB.Models;
using ServerPhB.Services;

namespace ServerPhB.Tests
{
    [TestFixture]
    public class UserControllerTests
    {
        private ApplicationDbContext _context;
        private AuthenticationService _authenticationService;
        private UserController _controller;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);

            var inMemorySettings = new Dictionary<string, string> {
                {"Jwt:Key", "Server_PhB_JWT_KEY_PASSWORD_PROTECTED_FOR_USERS"}
            };
            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _authenticationService = new AuthenticationService(_context, configuration);
            _controller = new UserController(_authenticationService);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "testUserId"),
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
        public async Task RefreshToken_ReturnsOkResult_WhenTokenIsValid()
        {
            // Arrange
            var user = new User
            {
                Username = "validuser",
                PasswordHash = "hashedpassword",
                Name = "Valid User",
                Email = "validuser@example.com",
                Phone = "1234567890",
                Role = 1
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var token = _authenticationService.GenerateJwtToken(user);

            var request = new RefreshTokenRequest
            {
                Token = token
            };

            // Act
            var result = await _controller.RefreshToken(request);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult.Value.GetType().GetProperty("token").GetValue(okResult.Value, null));
            Assert.IsNotNull(okResult.Value.GetType().GetProperty("refreshToken").GetValue(okResult.Value, null));
        }

        [Test]
        public async Task RefreshToken_ReturnsUnauthorized_WhenTokenIsInvalid()
        {
            // Arrange
            var request = new RefreshTokenRequest
            {
                Token = "invalid_refresh_token"
            };

            // Act
            var result = await _controller.RefreshToken(request);

            // Assert
            Assert.IsInstanceOf<UnauthorizedObjectResult>(result);
            var unauthorizedResult = result as UnauthorizedObjectResult;
            Assert.AreEqual("Invalid refresh token", unauthorizedResult.Value.GetType().GetProperty("message").GetValue(unauthorizedResult.Value, null));
        }
    }
}
