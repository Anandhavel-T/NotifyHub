using NotifyHub.Models.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotifyHub.Infrastructure.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> AuthenticateAsync(string username, string password);
        Task<User> CreateUserAsync(User user, string password);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
        Task<User> GetUserByIdAsync(int id);
        Task<IEnumerable<User>> GetAllUsersAsync();
        string GenerateJwtToken(User user);
        User GetByEmail(string email);
    }
}