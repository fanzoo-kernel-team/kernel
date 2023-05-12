namespace Fanzoo.Kernel.Services
{
    public interface IDateTimeOffsetService
    {
        DateTimeOffset Now { get; }

        DateTimeOffset UtcNow { get; }

        DateOnly Today { get; }

        DateOnly UtcToday { get; }

        TimeOnly Time { get; }

        TimeOnly UtcTime { get; }
    }

    public class DateTimeOffsetService : IDateTimeOffsetService
    {
        public DateTimeOffset Now => DateTimeOffset.Now;

        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;

        public DateOnly Today => DateOnly.FromDateTime(DateTimeOffset.Now.Date);

        public DateOnly UtcToday => DateOnly.FromDateTime(DateTimeOffset.UtcNow.Date);

        public TimeOnly Time => TimeOnly.FromTimeSpan(DateTimeOffset.Now.TimeOfDay);

        public TimeOnly UtcTime => TimeOnly.FromTimeSpan(DateTimeOffset.UtcNow.TimeOfDay);
    }
}
