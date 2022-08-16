namespace Fanzoo.Kernel.Domain.Values
{
    public interface IRoleValue<out TPrimitive>
    {
        TPrimitive Id { get; }

        string Name { get; }
    }
}
