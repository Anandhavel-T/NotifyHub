using Microsoft.IdentityModel.Tokens;
using NotifyHub.Infrastructure.Repositories.Interfaces;
using NotifyHub.Infrastructure.Services.Interfaces;
using NotifyHub.Models.Domain;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System;
using Microsoft.Ajax.Utilities;

namespace NotifyHub.Infrastructure.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly string _jwtSecret;
        private readonly bool _mfaEnabled;

        public UserService(
            IUserRepository userRepository,
            IEmailService emailService)
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _jwtSecret = ConfigurationManager.AppSettings["Jwt:Secret"];
            _mfaEnabled = bool.Parse(ConfigurationManager.AppSettings["Security:MfaEnabled"]);
        }

        public async Task<User> AuthenticateAsync(string username, string password)
        {
            var user = _userRepository.GetByUsername(username);

            if (user == null || user.PasswordHash.IsNullOrWhiteSpace() || !VerifyPasswordHash(password, user.PasswordHash))
                return null;

            user.LastLoginDate = DateTime.UtcNow;
            await UpdateUserAsync(user);

            return user;
        }

        public async Task<User> CreateUserAsync(User user, string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Password is required");

            if (_userRepository.GetByUsername(user.UserName) != null)
                throw new ArgumentException("Username is already taken");

            user.PasswordHash = HashPassword(password);
            user.CreatedAt = DateTime.UtcNow;
            user.IsActive = true;

            _userRepository.Insert(user);
            await _userRepository.SaveAsync();

            await _emailService.SendEmailAsync(
                user.Email,
                "Welcome to NotifyHub",
                $"Dear {user.FullName},\n\nYour account has been created successfully."
            );

            return user;
        }

        public Task DeleteUserAsync(int id)
        {
            _userRepository.DeleteAsync(id);
            _userRepository.SaveAsync();
            return Task.CompletedTask;
        }

        public string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public User GetByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public Task UpdateUserAsync(User user)
        {
            _userRepository.Update(user);
            _userRepository.SaveAsync();
            return Task.CompletedTask;
        }

        private string HashPassword(string password)
        {
            using (var hmac = new HMACSHA512())
            {
                var salt = hmac.Key;
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

                byte[] hashBytes = new byte[salt.Length + hash.Length];
                Array.Copy(salt, 0, hashBytes, 0, salt.Length);
                Array.Copy(hash, 0, hashBytes, salt.Length, hash.Length);

                return Convert.ToBase64String(hashBytes);
            }
        }

        private bool VerifyPasswordHash(string password, string storedHash)
        {
            byte[] hashBytes = Convert.FromBase64String(storedHash);
            byte[] salt = new byte[64];
            Array.Copy(hashBytes, 0, salt, 0, salt.Length);

            using (var hmac = new HMACSHA512(salt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != hashBytes[salt.Length + i]) return false;
                }
            }
            return true;
        }
    }
}