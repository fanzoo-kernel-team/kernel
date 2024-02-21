using Microsoft.AspNetCore.Identity;

namespace Fanzoo.Kernel.Services
{
    public class IdentityPasswordHashingService : IPasswordHashingService
    {
        private readonly PasswordHasher<string> _passwordHasher;

        public IdentityPasswordHashingService() => _passwordHasher = new PasswordHasher<string>();

        public string HashPassword(string username, string password) => _passwordHasher.HashPassword(username, password);

        //TODO: modify this to return an enum so our auth service can automatically rehash
        public bool VerifyPasswordHash(string username, string hashedPassword, string providedPassword) =>
            _passwordHasher.VerifyHashedPassword(username, hashedPassword, providedPassword) switch
            {
                PasswordVerificationResult.Failed => false,
                PasswordVerificationResult.Success or PasswordVerificationResult.SuccessRehashNeeded => true,
                _ => throw new InvalidOperationException()
            };
    }
}
