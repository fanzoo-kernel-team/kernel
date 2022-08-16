namespace Fanzoo.Kernel.Data.Listeners
{
    public static class NHibernateListenerExtensions
    {
        public static T? GetService<T>(this IDatabaseEventArgs @event) => ((KernelInterceptor)@event.Session.Interceptor).GetService<T>();
    }
}
