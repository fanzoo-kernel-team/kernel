using Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Entities;
using Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Values;

namespace Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Data.Repositories
{
    public sealed class GameRepository : NHibernateRepositoryCore<Game, GameIdentifierValue, Guid>, IGameRepository
    {
        public GameRepository(IUnitOfWorkFactory unitOfWorkFactory) : base(unitOfWorkFactory) { }
    }
}
