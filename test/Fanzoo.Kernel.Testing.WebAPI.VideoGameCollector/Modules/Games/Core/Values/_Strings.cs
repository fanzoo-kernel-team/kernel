namespace Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Modules.Games.Core.Values
{
    public class GameNameValue : RequiredStringValue
    {
        private GameNameValue() { } //ORM

        public GameNameValue(string value) : base(value, 1024) { }
    }
}
