namespace Fanzoo.Kernel.Services
{
    public interface IContextAccessorService
    {
        public ClaimsPrincipal? User { get; }
    }
}
