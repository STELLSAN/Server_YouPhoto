using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ServerPhB.Data;
using ServerPhB.Models;

namespace ServerPhB.Services
{
    public class AuthenticationService
    {
        private readonly ApplicationDbContext _context;
        private readonly string _secretKey;

        public AuthenticationService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _secretKey = configuration["Jwt:Key"];
        }

        public async Task<string> Register(string username, string password, string name, string email, string phone, int role)
        {
            if (await _context.Users.AnyAsync(u => u.Username == username))
            {
                return null; // Пользователь уже существует
            }

            var salt = GenerateSalt();
            var saltedPassword = HashPassword(password, salt);

            var user = new User
            {
                Username = username,
                PasswordHash = saltedPassword,
                Name = "null",
                Email = "null",
                Phone = "null",
                Role = role
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return GenerateJwtToken(user);
        }

        public async Task<string> Authenticate(string username, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                return null;
            }

            var salt = user.PasswordHash.Substring(0, 24);
            var saltedPassword = HashPassword(password, salt);
            if (user.PasswordHash != saltedPassword)
            {
                return null;
            }

            return GenerateJwtToken(user);
        }

        private string GenerateSalt()
        {
            var rng = new RNGCryptoServiceProvider();
            var saltBytes = new byte[16];
            rng.GetBytes(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }

        private string HashPassword(string password, string salt)
        {
            var sha256 = SHA256.Create();
            var saltedPassword = salt + password;
            var saltedPasswordBytes = Encoding.UTF8.GetBytes(saltedPassword);
            var hashBytes = sha256.ComputeHash(saltedPasswordBytes);
            return salt + Convert.ToBase64String(hashBytes);
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
