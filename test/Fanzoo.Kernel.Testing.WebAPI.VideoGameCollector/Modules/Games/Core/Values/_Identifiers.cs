namespace Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Modules.Games.Core.Values
{
    public class GameIdentifierValue : GuidIdentifierValue<GameIdentifierValue>
    {
        public GameIdentifierValue() : base() { }

        public GameIdentifierValue(Guid id) : base(id) { }
    }
}
