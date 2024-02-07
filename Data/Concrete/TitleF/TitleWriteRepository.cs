using Sozluk.Data.Abstract.Title;
using Sozluk.Entities;

namespace Sozluk.Data.Concrete.TitleF
{
    public class TitleWriteRepository : WriteRepository<Title>, ITitleWriteRepository
    {
        public TitleWriteRepository(SozlukContext context) : base(context)
        {
        }
    }
}
