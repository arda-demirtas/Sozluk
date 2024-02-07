using Microsoft.EntityFrameworkCore;
using Sozluk.Entities;

namespace Sozluk.Data.Concrete
{
    public class SozlukContext : DbContext
    {
        public SozlukContext(DbContextOptions<SozlukContext> options) : base(options) { }
        public DbSet<Entry> Entries => Set<Entry>();
        public DbSet<Title> Titles => Set<Title>();
        public DbSet<User> Users => Set<User>();
    }
}
