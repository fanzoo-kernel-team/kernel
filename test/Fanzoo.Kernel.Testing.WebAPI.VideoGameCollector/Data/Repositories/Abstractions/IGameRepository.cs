using Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Entities;
using Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Values;

namespace Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Data.Repositories
{
    public interface IGameRepository : IRepository<Game, GameIdentifierValue, Guid>
    {
        ValueTask<IEnumerable<Game>> FindByNameAsync(string name);
    }
}
