namespace Fanzoo.Kernel
{
    public class Guard
    {
        private Guard() { }

        public static Guard Against => new();
    }
}
