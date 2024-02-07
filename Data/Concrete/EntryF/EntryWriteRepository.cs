using Sozluk.Data.Abstract.Entry;
using Sozluk.Entities;

namespace Sozluk.Data.Concrete.EntryF
{
    public class EntryWriteRepository : WriteRepository<Entry>, IEntryWriteRepository
    {
        public EntryWriteRepository(SozlukContext context) : base(context)
        {
        }
    }
}
