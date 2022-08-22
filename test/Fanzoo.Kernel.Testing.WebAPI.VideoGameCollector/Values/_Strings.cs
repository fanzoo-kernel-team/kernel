namespace Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector.Values
{
    public class GameNameValue : StringValue
    {
        private GameNameValue() { } //ORM

        public GameNameValue(string value) : base(value, 1024) { }
    }
}
