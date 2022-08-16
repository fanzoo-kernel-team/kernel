using Microsoft.AspNetCore.Identity;

namespace Fanzoo.Kernel.Services
{
    public class IdentityPasswordHashingService : IPasswordHashingService
    {
        private readonly PasswordHasher<string> _passwordHasher;

        public IdentityPasswordHashingService()
        {
            _passwordHasher = new PasswordHasher<string>();
        }

        public string HashPassword(string username, string password) => _passwordHasher.HashPassword(username, password);

        public bool VerifyPasswordHash(string username, string hashedPassword, string providedPassword) => _passwordHasher.VerifyHashedPassword(username, hashedPassword, providedPassword) == PasswordVerificationResult.Success;
    }
}
