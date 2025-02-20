using NotifyHub.Models.Domain;

namespace NotifyHub.Infrastructure.Repositories.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        User GetByUsername(string username);
        User GetByEmail(string email);
    }
}
