using Fanzoo.Kernel.Domain.Values.Identifiers.Guid;
using Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Modules.Users.Core.Entities;
using NHibernate.Linq;

namespace Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Modules.Users.Data.Repositories
{
    public interface IUserRepository : IRepository<User, UserIdentifierValue, Guid>
    {
        ValueTask<User?> FindByUsername(EmailUsernameValue username);
    }

    public class UserRepository : NHibernateRepositoryCore<User, UserIdentifierValue, Guid>, IUserRepository
    {
        public UserRepository(IUnitOfWorkFactory unitOfWorkFactory) : base(unitOfWorkFactory) { }

        public async ValueTask<User?> FindByUsername(EmailUsernameValue username) => await Query.SingleOrDefaultAsync(u => u.Username == username);
    }
}
