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
