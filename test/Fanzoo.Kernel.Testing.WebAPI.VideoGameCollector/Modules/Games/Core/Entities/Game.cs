namespace Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Modules.Games.Core.Entities
{
    public class Game : AggregateRoot<GameIdentifierValue, Guid>, IMutableEntity
    {
        protected Game() { }

        public static Game Create(string name) => new()
        {
            Name = new(name)
        };

        public GameNameValue Name { get; set; } = default!;
    }
}
