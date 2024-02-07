using Sozluk.Data.Abstract.User;
using Sozluk.Entities;

namespace Sozluk.Data.Concrete.UserF
{
    public class UserWriteRepository : WriteRepository<User>, IUserWriteRepository
    {
        public UserWriteRepository(SozlukContext context) : base(context)
        {
        }
    }
}
