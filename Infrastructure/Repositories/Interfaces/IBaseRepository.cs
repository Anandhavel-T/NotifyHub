using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotifyHub.Infrastructure.Repositories.Interfaces
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(int id);
        void Insert(TEntity entity);
        void Update(TEntity entityToUpdate);
        Task DeleteAsync(int id);
        Task<bool> SaveAsync();
    }
}
