using Fanzoo.Kernel.Data.Mapping;
using Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Entities;
using Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Values;

namespace Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Data.Mapping
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
