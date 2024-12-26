using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using ServerPhB.Data;
using ServerPhB.Models;
using ServerPhB.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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

        [TearDown]
        public void Teardown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task Register_ReturnsNull_WhenUsernameAlreadyExists()
        {
            // Arrange
            _context.Users.Add(new User
            {
                Username = "existinguser",
                Email = "existinguser@email.com",
                Name = "Existing User",
                Phone = "1234567890",
                PasswordHash = "hashedpassword",
                Role = 1
            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _authService.Register("existinguser", "testpassword", "Test Name", "test@email.com", "1234567890", 1);

            // Assert
            Assert.IsNull(result.token);
            Assert.AreEqual(0, result.role);
        }

        [Test]
        public async Task Register_ReturnsTokenAndRole_WhenRegistrationIsSuccessful()
        {
            // Act
            var result = await _authService.Register("newuser", "testpassword", "Test Name", "test@email.com", "1234567890", 1);

            // Assert
            Assert.IsNotNull(result.token);
            Assert.AreEqual(1, result.role);
        }

        [Test]
        public async Task Authenticate_ReturnsTokenAndRole_WhenCredentialsAreValid()
        {
            // Arrange
            var salt = _authService.GenerateSalt();
            var passwordHash = _authService.HashPassword("validpassword", salt);

            var user = new User
            {
                Username = "validuser",
                Email = "validuser@email.com",
                Name = "Valid User",
                Phone = "1234567890",
                PasswordHash = passwordHash,
                Role = 1
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _authService.Authenticate("validuser", "validpassword");

            // Assert
            Assert.IsNotNull(result.token);
            Assert.AreEqual(1, result.role);
        }


        [Test]
        public async Task Authenticate_ReturnsNull_WhenUserDoesNotExist()
        {
            // Act
            var result = await _authService.Authenticate("nonexistentuser", "password");

            // Assert
            Assert.IsNull(result.token);
            Assert.AreEqual(0, result.role);
        }

        [Test]
        public async Task Authenticate_ReturnsNull_WhenPasswordIsIncorrect()
        {
            // Arrange
            var salt = _authService.GenerateSalt();
            var passwordHash = _authService.HashPassword("correctpassword", salt);

            var user = new User
            {
                Username = "validuser",
                Email = "validuser@email.com",
                Name = "Valid User",
                Phone = "1234567890",
                PasswordHash = passwordHash,
                Role = 1
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _authService.Authenticate("validuser", "wrongpassword");

            // Assert
            Assert.IsNull(result.token);
            Assert.AreEqual(0, result.role);
        }

        [Test]
        public async Task RefreshToken_ReturnsNewTokenAndRefreshToken_WhenTokenIsValid()
        {
            // Arrange
            var salt = _authService.GenerateSalt();
            var passwordHash = _authService.HashPassword("password", salt);

            var user = new User
            {
                Username = "validuser",
                Email = "validuser@email.com",
                Name = "Valid User",
                Phone = "1234567890",
                PasswordHash = passwordHash,
                Role = 1
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var token = _authService.GenerateJwtToken(user);

            // Act
            var result = await _authService.RefreshToken(token);

            // Assert
            Assert.IsNotNull(result.token);
            Assert.IsNotNull(result.refreshToken);
        }

        [Test]
        public void GetPrincipalFromExpiredToken_ReturnsPrincipal_WhenTokenIsValid()
        {
            // Arrange
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("Server_PhB_JWT_KEY_PASSWORD_PROTECTED_FOR_USERS");
            var validTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, "testuser") }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var validToken = tokenHandler.WriteToken(tokenHandler.CreateToken(validTokenDescriptor));

            // Act
            var principal = _authService.GetPrincipalFromExpiredToken(validToken);

            // Assert
            Assert.IsNotNull(principal);
            Assert.AreEqual("testuser", principal.Identity.Name);
        }

        [Test]
        public void GenerateRefreshToken_ReturnsUniqueTokens()
        {
            // Arrange

            // Act
            var token1 = _authService.GenerateRefreshToken();
            var token2 = _authService.GenerateRefreshToken();

            // Assert
            Assert.IsNotNull(token1);
            Assert.IsNotNull(token2);
            Assert.AreNotEqual(token1, token2);
        }

        [Test]
        public void GenerateRefreshToken_ReturnsBase64String()
        {
            // Arrange

            // Act
            var token = _authService.GenerateRefreshToken();

            // Assert
            Assert.DoesNotThrow(() => Convert.FromBase64String(token));
            Assert.AreEqual(32, Convert.FromBase64String(token).Length);
        }


        [Test]
        public void GenerateSalt_ReturnsUniqueSalt()
        {
            // Act
            var salt1 = _authService.GenerateSalt();
            var salt2 = _authService.GenerateSalt();

            // Assert
            Assert.IsNotNull(salt1);
            Assert.IsNotNull(salt2);
            Assert.AreNotEqual(salt1, salt2);
        }

        [Test]
        public void HashPassword_ReturnsConsistentHash_ForSameInput()
        {
            // Arrange
            var salt = _authService.GenerateSalt();

            // Act
            var hash1 = _authService.HashPassword("password", salt);
            var hash2 = _authService.HashPassword("password", salt);

            // Assert
            Assert.AreEqual(hash1, hash2);
        }

        [Test]
        public void HashPassword_ReturnsDifferentHash_ForDifferentInput()
        {
            // Arrange
            var salt = _authService.GenerateSalt();

            // Act
            var hash1 = _authService.HashPassword("password1", salt);
            var hash2 = _authService.HashPassword("password2", salt);

            // Assert
            Assert.AreNotEqual(hash1, hash2);
        }
    }
}
