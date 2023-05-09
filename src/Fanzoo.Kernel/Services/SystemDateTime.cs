﻿namespace Fanzoo.Kernel.Services
{
    public static class SystemDateTime
    {
        private static IDateTimeService? _dateTimeService;

        public static void SetProvider(IDateTimeService dateTimeService) => _dateTimeService = dateTimeService;

        public static DateTime Now => _dateTimeService != null ? _dateTimeService.Now : throw new Exception("date time provider not initialized");

        public static DateTime UtcNow => _dateTimeService != null ? _dateTimeService.UtcNow : throw new Exception("date time provider not initialized");

        public static DateOnly Today => _dateTimeService != null ? _dateTimeService.Today : throw new Exception("date time provider not initialized");

        public static DateOnly UtcToday => _dateTimeService != null ? _dateTimeService.UtcToday : throw new Exception("date time provider not initialized");

        public static TimeOnly Time => _dateTimeService != null ? _dateTimeService.Time : throw new Exception("date time provider not initialized");

        public static TimeOnly UtcTime => _dateTimeService != null ? _dateTimeService.UtcTime : throw new Exception("date time provider not initialized");

    }
}
