using Microsoft.EntityFrameworkCore;
using Sozluk.Entities.Common;

namespace Sozluk.Data.Abstract
{
    public interface IRepository<T> where T : Entity
    {
        DbSet<T> Table {  get; }
    }
}
