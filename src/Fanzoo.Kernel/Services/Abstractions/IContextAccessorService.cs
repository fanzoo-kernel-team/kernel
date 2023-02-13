namespace Fanzoo.Kernel.Services
{
    public interface IContextAccessorService
    {
        public ClaimsPrincipal? GetUser();

        public ValueTask<ClaimsPrincipal?> GetUserAsync();
    }
}
