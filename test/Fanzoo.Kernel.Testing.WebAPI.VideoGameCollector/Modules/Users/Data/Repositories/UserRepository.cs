using Fanzoo.Kernel.Defaults.Domain.Values.Identifiers;
using NHibernate.Linq;

namespace Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Modules.Users.Data.Repositories
{
    public interface IUserRepository : IRepository<User, UserIdentifierValue, Guid>
    {
        ValueTask<User?> FindByUsername(EmailUsernameValue username);

        ValueTask<User?> FindByRefreshToken(RefreshTokenValue refreshToken);
    }

    public class UserRepository(IUnitOfWorkFactory unitOfWorkFactory) : NHibernateRepositoryCore<User, UserIdentifierValue, Guid>(unitOfWorkFactory), IUserRepository
    {
        public async ValueTask<User?> FindByRefreshToken(RefreshTokenValue refreshToken) => await Query.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == refreshToken));

        public async ValueTask<User?> FindByUsername(EmailUsernameValue username) => await Query.SingleOrDefaultAsync(u => u.Username == username);
    }
}
