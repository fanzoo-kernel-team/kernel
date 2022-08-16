namespace Fanzoo.Kernel.Services
{
    public static class SystemDateTime
    {
        private static IDateTimeService? _dateTimeService;

        public static void SetProvider(IDateTimeService dateTimeService) => _dateTimeService = dateTimeService;

        public static DateTime Now => _dateTimeService != null ? _dateTimeService.Now : throw new Exception("date time provider not initialized");

    }
}
