using Sozluk.Data.Abstract.Title;
using Sozluk.Entities;

namespace Sozluk.Data.Concrete.TitleF
{
    public class TitleReadRepository : ReadRepository<Title>, ITitleReadRepository
    {
        public TitleReadRepository(SozlukContext context) : base(context)
        {
        }
    }
}
