using Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Entities;
using Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Values;
using NHibernate.Linq;

namespace Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Data.Repositories
{
    public sealed class GameRepository : NHibernateRepositoryCore<Game, GameIdentifierValue, Guid>, IGameRepository
    {
        public GameRepository(IUnitOfWorkFactory unitOfWorkFactory) : base(unitOfWorkFactory) { }

        public async ValueTask<IEnumerable<Game>> FindAsync(GameNameValue name) =>
            await Query
                .Where(game => game.Name == name)
                    .ToListAsync();
    }
}
