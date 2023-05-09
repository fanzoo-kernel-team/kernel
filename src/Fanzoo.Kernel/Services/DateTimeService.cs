namespace Fanzoo.Kernel.Services
{
    public interface IDateTimeService
    {
        DateTime Now { get; }

        DateTime UtcNow { get; }

        DateOnly Today { get; }

        DateOnly UtcToday { get; }

        TimeOnly Time { get; }

        TimeOnly UtcTime { get; }
    }

    public class DateTimeService : IDateTimeService
    {
        public DateTime Now => DateTime.Now;

        public DateTime UtcNow => DateTime.UtcNow;

        public DateOnly Today => DateOnly.FromDateTime(DateTime.Today);

        public DateOnly UtcToday => DateOnly.FromDateTime(DateTime.UtcNow);

        public TimeOnly Time => TimeOnly.FromDateTime(DateTime.Now);

        public TimeOnly UtcTime => TimeOnly.FromDateTime(DateTime.UtcNow);
    }
}
