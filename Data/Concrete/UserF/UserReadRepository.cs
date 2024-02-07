using Sozluk.Data.Abstract.User;
using Sozluk.Entities;

namespace Sozluk.Data.Concrete.UserF
{
    public class UserReadRepository : ReadRepository<User>, IUserReadRepository
    {
        public UserReadRepository(SozlukContext context) : base(context)
        {
        }
    }
}
