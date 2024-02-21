namespace Fanzoo.Kernel.Data
{
    public sealed class NHibernateUnitOfWorkFactory : IUnitOfWorkFactory
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
            //I may regret this
            if (_current is not null && !_current.IsClosed)
            {
                return _current;
            }

            if (!CanOpen)
            {
                throw new InvalidOperationException("Cannot open a new UnitOfWork until the current one is closed.");
            }

            _current?.Dispose();

            _current = new NHibernateUnitOfWork(
                _sessionFactory
                    .WithOptions()
                        .Interceptor(new KernelInterceptor(_serviceProvider))
                            .OpenSession());

            return _current;
        }

        public void Close() => _current?.Dispose();

        public IUnitOfWork Current => _current ?? throw new InvalidOperationException("No UnitOfWork exists.");

        public bool CanOpen => _current is null || _current.IsClosed;

        public bool HasUnitOfWork => _current is not null;

        public void Dispose() => _current?.Dispose();

        public ValueTask DisposeAsync()
        {
            Dispose();

            return ValueTask.CompletedTask;
        }
    }
}
