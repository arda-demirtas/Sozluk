using Microsoft.EntityFrameworkCore;
using Sozluk.Data.Abstract;
using Sozluk.Entities.Common;
using System.Linq.Expressions;

namespace Sozluk.Data.Concrete
{
    public class ReadRepository<T> : IReadRepository<T> where T : Entity
    {
        private readonly SozlukContext _context;
        public ReadRepository(SozlukContext context)
        {
            _context = context;
        }
        public DbSet<T> Table => _context.Set<T>();
        public IQueryable<T> GetAll()
        {
            return Table;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await Table.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> method)
        {
            return await Table.FirstOrDefaultAsync(method);
        }

        public IQueryable<T> GetWhere(Expression<Func<T, bool>> method)
        {
            return Table.Where(method);
        }
    }
}
