using Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Values;

namespace Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Entities
{
    public class Game : AggregateRoot<GameIdentifierValue, Guid>, IMutableEntity
    {
        protected Game() { }

        public static Game Create(string name)
        {
            return new()
            {
                Name = new(name)
            };
        }

        public GameNameValue Name { get; protected set; } = default!;
    }
}
