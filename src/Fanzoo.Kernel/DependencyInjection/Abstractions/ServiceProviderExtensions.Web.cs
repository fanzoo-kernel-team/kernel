namespace Fanzoo.Kernel.DependencyInjection
{
    public static partial class ServiceProviderExtensions
    {
        public static IServiceCollection AddWebCore(this IServiceCollection services) =>
            services
                .AddHttpContextAccessor()
                .AddTransient<IContextAccessorService, HttpContextAccessorService>()
                .AddTransient<ICurrentUserService, CurrentUserService>();

    }
}
