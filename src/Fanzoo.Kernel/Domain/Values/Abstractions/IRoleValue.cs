namespace Fanzoo.Kernel.Domain.Values
{
    public interface IRoleValue<out TPrimitive>
        where TPrimitive : notnull
    {
        TPrimitive Id { get; }

        string Name { get; }
    }
}
