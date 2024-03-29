﻿using Fanzoo.Kernel.Defaults.Domain.Values.Identifiers;
using NHibernate.Linq;

namespace Fanzoo.Kernel.Testing.Web.Razor.Modules.Users.Data.Repositories
{
    public interface IUserRepository : IRepository<User, UserIdentifierValue, Guid>
    {
        ValueTask<User?> FindByUsername(EmailUsernameValue username);
    }

    public class UserRepository(IUnitOfWorkFactory unitOfWorkFactory) : NHibernateRepositoryCore<User, UserIdentifierValue, Guid>(unitOfWorkFactory), IUserRepository
    {
        public async ValueTask<User?> FindByUsername(EmailUsernameValue username) => await Query.SingleOrDefaultAsync(u => u.Username == username);
    }
}
