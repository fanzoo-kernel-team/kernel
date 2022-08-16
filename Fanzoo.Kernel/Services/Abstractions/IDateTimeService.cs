namespace Fanzoo.Kernel.Services
{
    public interface IDateTimeService
    {
        public DateTime Now => DateTime.Now.ToUniversalTime();
    }
}
