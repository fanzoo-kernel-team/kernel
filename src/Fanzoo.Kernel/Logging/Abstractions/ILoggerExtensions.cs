using Fanzoo.Kernel.Commands;
using Fanzoo.Kernel.Events;
using Serilog;
using IQuery = Fanzoo.Kernel.Queries.IQuery; // It's not clear to me why it's trying to use NHibernate.IQuery here.

namespace Fanzoo.Kernel.Logging
{
    public static class ILoggerExtensions
    {
        private static string GetCommandName<TCommand>() where TCommand : ICommand =>
            typeof(TCommand)
                .FullName!
                    .Split('.')
                        .TakeLast(3)
                            .Join('.')
                                .Replace("Commands.", "");

        public static void CommandInformation<TCommand>(this ILogger logger, string? message) where TCommand : ICommand =>
            logger.Information($"[{GetCommandName<TCommand>()}] {message}");

        public static void CommandWarning<TCommand>(this ILogger logger, string? message) where TCommand : ICommand =>
            logger.Warning($"[{GetCommandName<TCommand>()}] {message}");

        public static void CommandException<TCommand>(this ILogger logger, Exception exception) where TCommand : ICommand =>
            logger.Error(exception, $"[{GetCommandName<TCommand>()}] {exception.Message}");


        private static string GetEventName<TEvent>() where TEvent : IEvent =>
            typeof(TEvent)
                .FullName!
                    .Split('.')
                        .TakeLast(3)
                            .Join('.')
                                .Replace("Events.", "");

        public static void EventInformation<TEvent>(this ILogger logger, string? message) where TEvent : IEvent =>
            logger.Information($"[{GetEventName<TEvent>()}] {message}");

        public static void EventWarning<TEvent>(this ILogger logger, string? message) where TEvent : IEvent =>
            logger.Warning($"[{GetEventName<TEvent>()}] {message}");

        public static void EventException<TEvent>(this ILogger logger, Exception exception) where TEvent : IEvent =>
            logger.Error(exception, $"[{GetEventName<TEvent>()}] {exception.Message}");

        private static string GetQueryName<TQuery>() where TQuery : IQuery =>
            typeof(TQuery)
                .FullName!
                    .Split('.')
                        .TakeLast(3)
                            .Join('.')
                                .Replace("Queries.", "");

        public static void QueryInformation<TQuery>(this ILogger logger, string? message) where TQuery : IQuery =>
            logger.Information($"[{GetQueryName<TQuery>()}] {message}");

        public static void QueryWarning<TQuery>(this ILogger logger, string? message) where TQuery : IQuery =>
            logger.Warning($"[{GetQueryName<TQuery>()}] {message}");

        public static void QueryException<TQuery>(this ILogger logger, Exception exception) where TQuery : IQuery =>
            logger.Error(exception, $"[{GetQueryName<TQuery>()}] {exception.Message}");

    }
}
