using NHibernate.Linq;

namespace Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Modules.Games.Data.Repositories
{
    public interface IGameRepository : IRepository<Game, GameIdentifierValue, Guid>
    {
        ValueTask<IEnumerable<Game>> FindAsync(GameNameValue name);
    }

    public sealed class GameRepository(IUnitOfWorkFactory unitOfWorkFactory) : NHibernateRepositoryCore<Game, GameIdentifierValue, Guid>(unitOfWorkFactory), IGameRepository
    {
        public async ValueTask<IEnumerable<Game>> FindAsync(GameNameValue name) =>
            await Query
                .Where(game => game.Name == name)
                    .ToListAsync();
    }
}
