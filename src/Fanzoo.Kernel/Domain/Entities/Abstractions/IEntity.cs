namespace Fanzoo.Kernel.Domain.Entities
{
    public interface IEntity<out TIdentifier, TPrimitive>
        where TIdentifier : notnull, IdentifierValue<TPrimitive>
        where TPrimitive : notnull, new()
    {
        TIdentifier Id { get; }
    }
}
