namespace Fanzoo.Kernel.Services
{
    public interface IPasswordHashingService
    {
        string HashPassword(string username, string password);

        bool VerifyPasswordHash(string username, string hashedPassword, string providedPassword);
    }
}
