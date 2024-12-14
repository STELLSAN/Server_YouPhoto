using System;
using NUnit.Framework;
using ServerPhB.Services;
using ServerPhB.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using ServerPhB.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace ServerPhB.Tests
{
    [TestFixture]
    public class AuthenticationServiceTests
    {
        private AuthenticationService _authService;
        private ApplicationDbContext _context;

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

            _authService = new AuthenticationService(_context, configuration);
        }

        [Test]
        public async Task Authenticate_WithValidCredentials_ReturnsToken()
        {
            // Arrange
            var user = new User
            {
                Username = "testuser",
                PasswordHash = _authService.HashPassword("password", _authService.GenerateSalt()),
                Email = "testuser@example.com",
                Name = "Test User",
                Phone = "1234567890"
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Act
            var token = await _authService.Authenticate("testuser", "password");

            // Assert
            Assert.IsNotNull(token);
        }

        [Test]
        public async Task Authenticate_WithInvalidCredentials_ReturnsNull()
        {
            // Act
            var token = await _authService.Authenticate("invaliduser", "invalidpassword");

            // Assert
            Assert.IsNull(token);
        }
    }
}
