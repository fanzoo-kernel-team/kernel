using Fanzoo.Kernel.Data.Mapping;
using Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Modules.Games.Core.Entities;
using Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Modules.Games.Core.Values;

namespace Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Modules.Games.Data.Mapping
{
    public class GameMap : MutableEntityClassMap<Game, GameIdentifierValue, Guid>
    {
        public GameMap() : base()
        {
            MapValueObject(e => e.Name);

            Not.LazyLoad();
        }
    }
}
