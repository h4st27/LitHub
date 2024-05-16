using Libra.Dtos;
using Libra.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Libra.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly List<UserData> _users = new List<UserData>{
            new UserData
            {
                FirstName = "Yaroslav",
                LastName = "Popov",
                Email = "popov.yarik.popov@gmail.com",
                DateOfBirth = new DateTime(2004, 4, 22),
                HashedPassword = "ef797c8118f02dfb649607dd5d3f8c7623048c9c063d532cc95c5ed7a898a64f",
                LastLoginDate = DateTime.UtcNow,
                LoginAttempts = 0
            },
            new UserData
            {
                FirstName = "Alice",
                LastName = "Smith",
                Email = "alice@example.com",
                DateOfBirth = new DateTime(1985, 10, 20),
                HashedPassword = "ef797c8118f02dfb649607dd5d3f8c7623048c9c063d532cc95c5ed7a898a64f",
                LastLoginDate = DateTime.UtcNow.AddDays(-1),
                LoginAttempts = 0
            },
            new UserData
            {
                FirstName = "Bob",
                LastName = "Johnson",
                Email = "bob@example.com",
                DateOfBirth = new DateTime(1982, 8, 25),
                HashedPassword = "ef797c8118f02dfb649607dd5d3f8c7623048c9c063d532cc95c5ed7a898a64f",
                LastLoginDate = DateTime.UtcNow.AddDays(-2),
                LoginAttempts = 0
            }
        };
        private readonly IConfiguration _configuration;
        private readonly byte[] _secretKey;
        public UserService(IConfiguration configuration)
        {
            _configuration = configuration;
            _secretKey = Encoding.UTF8.GetBytes(configuration.GetSection("Authentication")["Secret"]);
        }


        private string GenerateToken(UserData user)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(_secretKey);
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
                new Claim(ClaimTypes.Email, user.Email)
                };

                var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.UtcNow.AddDays(1),
                    signingCredentials: credentials
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

    public string Login(AuthPayloadDto authPayload)
        {
            UserData user = ValidateUser(authPayload);
            if (user == null)
            {
                return null;
            }
            return GenerateToken(user);
        }

        public string Registrate(UserData user)
        {
            if (_users.Exists(u => u.Email == user.Email))
            {
                return null;
            }

            _users.Add(user);
            return GenerateToken(user);
        }

        public UserData ValidateUser(AuthPayloadDto authPayload)
        {
            UserData existingUser = _users.Find(u => u.Email == authPayload.Email);
            Console.WriteLine($"Wrong cred:{existingUser.LoginAttempts}");
            if (existingUser == null)
            {
                return null;
            }

            if (HashPassword(authPayload.Password) == existingUser.HashedPassword)
            {
                existingUser.LastLoginDate = DateTime.UtcNow;
                existingUser.LoginAttempts = 0;
                return existingUser;
            }
            existingUser.LoginAttempts += 1;
            return null;
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
