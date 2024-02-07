using Sozluk.Data.Abstract;
using Sozluk.Data.Abstract.Entry;
using Sozluk.Entities;

namespace Sozluk.Data.Concrete.EntryF
{
    public class EntryReadRepository : ReadRepository<Entry>, IEntryReadRepository
    {
        public EntryReadRepository(SozlukContext context) : base(context)
        {
        }
    }
}
