namespace Fanzoo.Kernel.Data
{
    public sealed class NHibernateUnitOfWorkFactory : IUnitOfWorkFactory, IDisposable, IAsyncDisposable
    {
        private readonly ISessionFactory _sessionFactory;
        private readonly IServiceProvider _serviceProvider;

        private NHibernateUnitOfWork? _current;

        public NHibernateUnitOfWorkFactory(ISessionFactory sessionFactory, IServiceProvider serviceProvider)
        {
            _sessionFactory = sessionFactory ?? throw new ArgumentNullException(nameof(sessionFactory));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public IUnitOfWork Open()
        {
            if (!CanOpen)
            {
                throw new InvalidOperationException("Cannot open a new UnitOfWork until the current one is closed.");
            }

            if (_current is not null)
            {
                _current.Dispose();
            }

            _current = new NHibernateUnitOfWork(
                _sessionFactory
                    .WithOptions()
                        .Interceptor(new KernelInterceptor(_serviceProvider))
                            .OpenSession());

            return _current;
        }

        public IUnitOfWork Current => _current ?? throw new InvalidOperationException("No UnitOfWork exists.");

        public bool CanOpen => _current is null || _current.IsClosed;

        public void Dispose()
        {
            if (_current is not null)
            {
                _current.Dispose();
            }
        }

        public ValueTask DisposeAsync()
        {
            Dispose();

            return ValueTask.CompletedTask;
        }
    }
}
