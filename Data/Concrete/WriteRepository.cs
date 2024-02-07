using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Sozluk.Data.Abstract;
using Sozluk.Entities.Common;

namespace Sozluk.Data.Concrete
{
    public class WriteRepository<T> : IWriteRepository<T> where T : Entity
    {
        private readonly SozlukContext _context;
        public WriteRepository(SozlukContext context)
        {
            _context = context;
        }

        public DbSet<T> Table => _context.Set<T>();

        public async Task<bool> AddAsync(T model)
        {
            EntityEntry<T> entryEntity = await Table.AddAsync(model);
            return entryEntity.State == EntityState.Added;
        }

        public async Task<bool> AddAsync(List<T> models)
        {
            await Table.AddRangeAsync(models);
            return true;
        }

        public bool Delete(T model)
        {
            EntityEntry<T> entryEntity = Table.Remove(model);
            return entryEntity.State == EntityState.Deleted;
        }

        public async Task<bool> Delete(int id)
        {
            var model = await Table.FirstOrDefaultAsync(x => x.Id == id);
            EntityEntry<T> entityEntry = Table.Remove(model);
            return entityEntry.State == EntityState.Deleted;
        }

        public bool Update(T model)
        {
            EntityEntry<T> entityEntry = Table.Update(model);
            return entityEntry.State == EntityState.Modified;
        }
        public async Task<int> SaveAsync()
        {
            int i = await _context.SaveChangesAsync();
            return i;
        }
    }
}
