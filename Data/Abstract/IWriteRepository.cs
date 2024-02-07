using Sozluk.Entities.Common;

namespace Sozluk.Data.Abstract
{
    public interface IWriteRepository<T> : IRepository<T> where T : Entity
    {
        Task<bool> AddAsync(T model);
        Task<bool> AddAsync(List<T> models);
        bool Delete(T model);
        Task<bool> Delete(int id);
        bool Update(T model);
        Task<int> SaveAsync();
    }
}
