using Fanzoo.Kernel.Commands;
using Fanzoo.Kernel.Data;
using Fanzoo.Kernel.Events;
using Fanzoo.Kernel.Queries;

namespace Fanzoo.Kernel.DependencyInjection
{
    public static partial class ServiceProviderExtensions
    {
        public static IServiceCollection AddFrameworkCore(this IServiceCollection services, Action<IServiceTypeAssemblyBuilder>? addTypes = null)
        {
            //add all the core stuff
            services
                .AddCommandHandlers(new[] { typeof(ICommandHandler<,>).Assembly })
                .AddEventHandlers(new[] { typeof(IEventHandler<>).Assembly })
                .AddQueryHandlers(new[] { typeof(IQueryHandler<,>).Assembly })
                .AddRepositories(new[] { typeof(IRepository<,,>).Assembly })
                .AddSlapper()
                .AddServices(new[] { typeof(IService).Assembly })
                .AddCore();

            //add configured stuff
            if (addTypes is not null)
            {
                var serviceTypeBuilder = new ServiceTypeAssemblyBuilder();

                addTypes.Invoke(serviceTypeBuilder);

                services.AddFrameworkCore(serviceTypeBuilder.Assemblies);
            }

            return services;
        }

        public static IServiceCollection AddScriptEmbeddedResourceLocator(this IServiceCollection services, Assembly assembly) =>
            services.AddSingleton<IScriptEmbeddedResourceLocator>(new EmbeddedResourceLocator(assembly));

        public static IServiceCollection AddHtmlTemplateGeneration(this IServiceCollection services, Assembly assembly) =>
            services
                .AddSingleton<ITemplateEmbeddedResourceLocator>(new EmbeddedResourceLocator(assembly))
                .AddTransient<IHtmlTemplateGenerationService, HtmlTemplateGenerationService>();

        internal static IServiceCollection AddFrameworkCore(this IServiceCollection services, IEnumerable<Assembly> assembliesToSearch) => services
                .AddCommandHandlers(assembliesToSearch)
                .AddEventHandlers(assembliesToSearch)
                .AddQueryHandlers(assembliesToSearch)
                .AddRepositories(assembliesToSearch)
                .AddServices(assembliesToSearch)
                .AddCore();

        internal static IServiceCollection AddCore(this IServiceCollection services) =>
            services
                .AddDateTimeService()
                .AddTransient<CommandDispatcher>()
                .AddTransient<QueryDispatcher>()
                .AddScoped<EventDispatcher>();

        internal static IServiceCollection AddCommandHandlers(this IServiceCollection services, IEnumerable<Assembly> assembliesToSearch)
        {
            static bool IsHandlerInterface(Type type) => type.IsGenericType &&
                        (type.GetGenericTypeDefinition() == typeof(ICommandHandler<>) || type.GetGenericTypeDefinition() == typeof(ICommandHandler<,>));

            foreach (var assembly in assembliesToSearch)
            {
                var commandHandlers = assembly.GetTypes()
                    .Where(t => !t.IsAbstract)
                    .Where(t => t.GetInterfaces().Any(i => IsHandlerInterface(i)))
                        .Select(t => (Type: t, Interface: t.GetInterfaces().Single(i => IsHandlerInterface(i))));

                foreach (var commandHandler in commandHandlers)
                {
                    services.AddTransient(commandHandler.Interface, commandHandler.Type);
                }
            }

            return services;
        }

        internal static IServiceCollection AddQueryHandlers(this IServiceCollection services, IEnumerable<Assembly> assembliesToSearch)
        {
            static bool IsHandlerInterface(Type type) => type.IsGenericType &&
                        (type.GetGenericTypeDefinition() == typeof(IQueryHandler<,>));

            foreach (var assembly in assembliesToSearch)
            {
                var queryHandlers = assembly.GetTypes()
                    .Where(t => !t.IsAbstract)
                    .Where(t => t.GetInterfaces().Any(i => IsHandlerInterface(i)))
                        .Select(t => (Type: t, Interface: t.GetInterfaces().Single(i => IsHandlerInterface(i))));

                foreach (var queryHandler in queryHandlers)
                {
                    services.AddTransient(queryHandler.Interface, queryHandler.Type);
                }
            }

            return services;

        }

        internal static IServiceCollection AddEventHandlers(this IServiceCollection services, IEnumerable<Assembly> assembliesToSearch)
        {
            static bool IsHandlerInterface(Type type) => type.IsGenericType &&
                        (type.GetGenericTypeDefinition() == typeof(IEventHandler<>));

            foreach (var assembly in assembliesToSearch)
            {
                var eventHandlers = assembly.GetTypes()
                    .Where(t => !t.IsAbstract)
                    .Where(t => t.GetInterfaces().Any(i => IsHandlerInterface(i)));

                foreach (var eventHandler in eventHandlers)
                {
                    services.AddTransient(eventHandler);
                }
            }

            return services;

        }

        internal static IServiceCollection AddRepositories(this IServiceCollection services, IEnumerable<Assembly> assembliesToSearch)
        {
            static bool IsRepositoryInterface(Type type) => type.IsGenericType &&
                        (type.GetGenericTypeDefinition() == typeof(IRepository<,,>));

            foreach (var assembly in assembliesToSearch)
            {
                var repositories = assembly.GetTypes()
                    .Where(t => !t.IsAbstract)
                    .Where(t => t.GetInterfaces().Any(i => IsRepositoryInterface(i)))
                        .Select(t => (Type: t, Interface: t.GetInterfaces().Single(i => !IsRepositoryInterface(i)))); //TODO: this assumes there are only 2 interfaces - the framework and the explicit. Problem down the road?

                foreach (var repository in repositories)
                {
                    services.AddTransient(repository.Interface, repository.Type);
                }
            }

            return services;
        }

        internal static IServiceCollection AddServices(this IServiceCollection services, IEnumerable<Assembly> assembliesToSearch)
        {
            foreach (var assembly in assembliesToSearch)
            {
                var applicationServices = assembly.GetTypes()
                    .Where(t => t.IsClass)
                    .Where(t => t.GetInterfaces().Any(i => i == typeof(IService)))
                        .Select(t => (Type: t, Interface: t.GetInterfaces().Except(new[] { typeof(IService) }).First()));

                foreach (var applicationService in applicationServices)
                {
                    if (services.Any(s => s.ServiceType == applicationService.Interface))
                    {
                        throw new ArgumentOutOfRangeException($"Trying to add {applicationService.Interface} more than once.");
                    }

                    services.AddTransient(applicationService.Interface, applicationService.Type);
                }
            }

            return services;
        }

        internal static IServiceCollection AddDateTimeService(this IServiceCollection services)
        {
            var dateTimeService = new DateTimeService();

            SystemDateTime.SetProvider(dateTimeService);

            return services.AddSingleton(typeof(IDateTimeService), dateTimeService);
        }

        internal static IServiceCollection AddSlapper(this IServiceCollection services) => services.AddTransient<IDynamicMappingService, SlapperDynamicMappingService>();

        internal static IServiceCollection AddJobs(this IServiceCollection services) => services; //TODO: add job stuff when the time comes
    }
}
