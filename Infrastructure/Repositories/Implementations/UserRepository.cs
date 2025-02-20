using NotifyHub.Data;
using NotifyHub.Infrastructure.Repositories.Interfaces;
using NotifyHub.Models.Domain;
using System.Linq;

namespace NotifyHub.Infrastructure.Repositories.Implementations
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context) { }

        public User GetByUsername(string username)
        {
            return _dbSet.FirstOrDefault(u => u.UserName == username);
        }

        public User GetByEmail(string email)
        {
            return _dbSet.FirstOrDefault(u => u.Email == email);
        }
    }
}