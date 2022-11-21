using Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Modules.Games.Core.Entities;
using Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Modules.Games.Core.Values;
using NHibernate.Linq;

namespace Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Modules.Games.Data.Repositories
{
    public interface IGameRepository : IRepository<Game, GameIdentifierValue, Guid>
    {
        ValueTask<IEnumerable<Game>> FindAsync(GameNameValue name);
    }

    public sealed class GameRepository : NHibernateRepositoryCore<Game, GameIdentifierValue, Guid>, IGameRepository
    {
        public GameRepository(IUnitOfWorkFactory unitOfWorkFactory) : base(unitOfWorkFactory) { }

        public async ValueTask<IEnumerable<Game>> FindAsync(GameNameValue name) =>
            await Query
                .Where(game => game.Name == name)
                    .ToListAsync();
    }
}
